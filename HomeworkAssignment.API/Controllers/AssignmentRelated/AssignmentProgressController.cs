using HomeAssignment.DTOs.RespondDTOs.AssignmentRelated;
using HomeworkAssignment.Application.Abstractions.AssignmentRelated;
using HomeworkAssignment.Controllers.Abstractions;
using HomeworkAssignment.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Hybrid;

namespace HomeworkAssignment.Controllers.AssignmentRelated;

[Produces("application/json")]
[Authorize]
[ApiController]
[Route("api/courses/{courseId:guid}/chapters/{chapterId:guid}/assignments/{assignmentId:guid}/attempts")]
public class AssignmentProgressController(
    IAssignmentProgressService service,
    HybridCache cache,
    ICacheKeyManager cacheKeyManager) : BaseController
{
    private readonly HybridCacheEntryOptions _cacheOptions = new()
    {
        LocalCacheExpiration = TimeSpan.FromMinutes(2),
        Expiration = TimeSpan.FromMinutes(5)
    };

    /// <summary>
    ///     Obtaining student progress on an assignment.
    /// </summary>
    [HttpGet("progress")]
    public async Task<ActionResult<RespondAssignmentUserProgressDto>> GetProgress(Guid courseId, Guid chapterId,
        Guid assignmentId,
        CancellationToken cancellationToken = default)
    {
        var userId = GetUserId();
        var cacheKey = cacheKeyManager.AssignmentProgress(userId, courseId, chapterId, assignmentId);

        var cachedProgress = await cache.GetOrCreateAsync(
            cacheKey,
            async _ => await service.GetProgressByAssignmentIdAsync(userId, assignmentId, cancellationToken),
            _cacheOptions,
            [cacheKeyManager.AssignmentSingleGroup(courseId, chapterId, assignmentId)],
            cancellationToken);

        return Ok(cachedProgress);
    }

    /// <summary>
    ///     Completion of the task.
    /// </summary>
    [HttpPatch("{attemptId:guid}/finish")]
    public async Task<ActionResult<IReadOnlyList<string>>> FinishAssignment(Guid courseId, Guid chapterId,
        Guid assignmentId,
        CancellationToken cancellationToken = default)
    {
        var userId = GetUserId();
        var result = await service.UpdateProgressAsync(userId, assignmentId, true, cancellationToken);

        await cache.RemoveByTagAsync(cacheKeyManager.AssignmentSingleGroup(courseId, chapterId, assignmentId),
            cancellationToken);
        return Ok(result);
    }

    /// <summary>
    ///     Retrying the task.
    /// </summary>
    [HttpPatch("{attemptId:guid}/restore")]
    public async Task<ActionResult<IReadOnlyList<string>>> RestoreAssignment(Guid courseId, Guid chapterId,
        Guid assignmentId,
        CancellationToken cancellationToken = default)
    {
        var userId = GetUserId();
        var result = await service.UpdateProgressAsync(userId, assignmentId, false, cancellationToken);

        await cache.RemoveByTagAsync(cacheKeyManager.AssignmentSingleGroup(courseId, chapterId, assignmentId),
            cancellationToken);
        return Ok(result);
    }
}