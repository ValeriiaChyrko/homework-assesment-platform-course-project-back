using AutoMapper;
using FluentValidation;
using HomeAssignment.DTOs.RequestDTOs.AttemptRelated;
using HomeAssignment.DTOs.RespondDTOs.AttemptRelated;
using HomeworkAssignment.Application.Abstractions.AttemptRelated;
using HomeworkAssignment.Controllers.Abstractions;
using HomeworkAssignment.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Hybrid;

namespace HomeworkAssignment.Controllers.AttemptRelated;

/// <summary>
///     A controller for managing student attempts.
/// </summary>
[Produces("application/json")]
[Authorize]
[ApiController]
[Route("api/courses/{courseId:guid}/chapters/{chapterId:guid}/assignments/{assignmentId:guid}/attempts")]
public class AttemptsController(
    IAttemptService attemptService,
    IAccountGrpcService accountGrpc,
    ICompilationGrpcService compilationGrpc,
    IQualityGrpcService qualityGrpc,
    ITestsGrpcService testsGrpc,
    IMapper mapper,
    HybridCache cache,
    ICacheKeyManager cacheKeyManager)
    : BaseController
{
    private readonly HybridCacheEntryOptions _cacheOptions = new()
    {
        LocalCacheExpiration = TimeSpan.FromMinutes(2),
        Expiration = TimeSpan.FromMinutes(5)
    };

    /// <summary>
    ///     Get a list of all attempts for a specific task.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<RespondAttemptDto>>> GetAttempts(Guid courseId, Guid chapterId,
        Guid assignmentId,
        CancellationToken cancellationToken = default)
    {
        var userId = GetUserId();

        var cacheKey = cacheKeyManager.AttemptList(courseId, chapterId, assignmentId);

        var cachedAttempts = await cache.GetOrCreateAsync(
            cacheKey,
            async _ => await attemptService.GetAttemptsByAssignmentIdAsync(userId, assignmentId, cancellationToken),
            _cacheOptions,
            [cacheKeyManager.AssignmentSingleGroup(courseId, chapterId, assignmentId)],
            cancellationToken);

        return Ok(cachedAttempts);
    }

    /// <summary>
    ///     Creating a new attempt to complete a task.
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<RespondAttemptDto>> Create(Guid courseId, Guid chapterId, Guid assignmentId,
        CancellationToken cancellationToken = default)
    {
        var userId = GetUserId();

        var result = await attemptService.CreateAttemptAsync(userId, assignmentId, cancellationToken);

        await cache.RemoveByTagAsync(cacheKeyManager.AssignmentSingleGroup(courseId, chapterId, assignmentId),
            cancellationToken);
        return Ok(result);
    }

    /// <summary>
    ///     Get a list of branches of a user's repository.
    /// </summary>
    [HttpPost("{attemptId:guid}/branches")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetAuthorBranches([FromBody] RequestBranchDto query,
        [FromServices] IValidator<RequestBranchDto> validator, CancellationToken cancellationToken = default)
    {
        var validationResult = await validator.ValidateAsync(query, cancellationToken);
        if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

        var result = await accountGrpc.GetBranchesAsync(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    ///     Update information about the task attempt.
    /// </summary>
    [HttpPatch("{attemptId:guid}")]
    public async Task<ActionResult<RespondAttemptDto>> Update(Guid courseId, Guid chapterId, Guid assignmentId,
        Guid attemptId,
        [FromBody] RequestPartialAttemptDto request, [FromServices] IValidator<RequestPartialAttemptDto> validator,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

        var userId = GetUserId();
        var result =
            await attemptService.UpdateAttemptAsync(userId, assignmentId, attemptId, request, cancellationToken);

        await cache.RemoveByTagAsync(cacheKeyManager.AssignmentSingleGroup(courseId, chapterId, assignmentId),
            cancellationToken);
        return Ok(result);
    }

    /// <summary>
    ///     Submit an attempt for automatic verification.
    /// </summary>
    [HttpPut("{attemptId:guid}/submit")]
    public async Task<ActionResult<RespondAttemptDto>> Submit(
        Guid courseId, Guid chapterId, Guid assignmentId, Guid attemptId,
        [FromBody] RequestSubmitAttemptDto request,
        [FromServices] IValidator<RequestSubmitAttemptDto> validator,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

        var dto = mapper.Map<RequestRepositoryWithBranchDto>(request);
        var scores = new List<ushort>();

        if (request.Assignment.AttemptCompilationSectionEnable)
        {
            var score = await GetScoreAsync(compilationGrpc.VerifyProjectCompilation, dto,
                request.Assignment.AttemptCompilationMinScore, request.Assignment.AttemptCompilationMaxScore,
                cancellationToken);
            scores.Add(score);
        }

        if (request.Assignment.AttemptQualitySectionEnable)
        {
            var score = await GetScoreAsync(qualityGrpc.VerifyProjectQualityAsync, dto,
                request.Assignment.AttemptQualityMinScore, request.Assignment.AttemptQualityMaxScore,
                cancellationToken);
            scores.Add(score);
        }

        if (request.Assignment.AttemptTestsSectionEnable)
        {
            var score = await GetScoreAsync(testsGrpc.VerifyProjectPassedTestsAsync, dto,
                request.Assignment.AttemptTestsMinScore, request.Assignment.AttemptTestsMaxScore,
                cancellationToken);
            scores.Add(score);
        }

        UpdateScores(request, scores.ToArray());

        var userId = GetUserId();
        var result = await attemptService.SubmitAttemptAsync(userId, assignmentId, attemptId, request,
            cancellationToken);

        await cache.RemoveByTagAsync(cacheKeyManager.AssignmentSingleGroup(courseId, chapterId, assignmentId),
            cancellationToken);
        return Ok(result);
    }

    private static void UpdateScores(RequestSubmitAttemptDto request, ushort[] scores)
    {
        var index = 0;
        if (request.Assignment.AttemptCompilationSectionEnable) request.Attempt.CompilationScore = scores[index++];
        if (request.Assignment.AttemptQualitySectionEnable) request.Attempt.QualityScore = scores[index++];
        if (request.Assignment.AttemptTestsSectionEnable) request.Attempt.TestsScore = scores[index];
    }

    private static async Task<ushort> GetScoreAsync(
        Func<RequestRepositoryWithBranchDto, CancellationToken, Task<int>> scoreFunc,
        RequestRepositoryWithBranchDto dto,
        int minScore,
        int maxScore,
        CancellationToken cancellationToken)
    {
        var percentage = await scoreFunc(dto, cancellationToken);
        return (ushort)Math.Round(minScore + percentage / 100.0 * (maxScore - minScore));
    }
}