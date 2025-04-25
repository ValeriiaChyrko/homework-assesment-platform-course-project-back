using AutoMapper;
using HomeAssignment.Domain.Abstractions;
using HomeAssignment.DTOs.RequestDTOs.AssignmentRelated;
using HomeAssignment.DTOs.RespondDTOs.AssignmentRelated;
using HomeAssignment.DTOs.RespondDTOs.AttemptRelated;
using HomeAssignment.Persistence.Commands.Assignments;
using HomeAssignment.Persistence.Queries.Assignments;
using HomeAssignment.Persistence.Queries.Attempts;
using HomeAssignment.Persistence.Queries.Users;
using HomeworkAssignment.Application.Abstractions;
using HomeworkAssignment.Application.Abstractions.AssignmentRelated;
using HomeworkAssignment.Application.Abstractions.ChapterRelated;
using HomeworkAssignment.Application.Abstractions.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HomeworkAssignment.Application.Implementations.AssignmentRelated;

public class AssignmentService(
    IChapterService chapterService,
    IDatabaseTransactionManager transactionManager,
    IMediator mediator,
    ILogger<AssignmentService> logger,
    IMapper mapper)
    : BaseService<AssignmentService>(logger, transactionManager), IAssignmentService
{
    private readonly ILogger<AssignmentService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<RespondAssignmentDto> CreateAssignmentAsync(
        Guid userId,
        Guid chapterId,
        RequestCreateAssignmentDto createAssignmentDto,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting creation of assignment in Chapter {ChapterId}", chapterId);

        var lastAssignment = await ExecuteTransactionAsync(
            async () => await mediator.Send(new GetLastAssignmentByIdQuery(chapterId), cancellationToken),
            cancellationToken: cancellationToken);

        var newPosition = (ushort)(lastAssignment?.Position + 1 ?? 1);

        var assignment = mapper.Map<Assignment>(createAssignmentDto);
        assignment.Position = newPosition;
        assignment.ChapterId = chapterId;

        assignment = await ExecuteTransactionAsync(
            async () => await mediator.Send(new CreateAssignmentCommand(assignment), cancellationToken),
            cancellationToken: cancellationToken);

        _logger.LogInformation("Successfully created assignment {AssignmentId} in Chapter {ChapterId}", assignment.Id,
            chapterId);
        return mapper.Map<RespondAssignmentDto>(assignment);
    }

    public async Task<RespondAssignmentDto> UpdateAssignmentAsync(Guid userId, Guid chapterId, Guid assignmentId,
        RequestPartialAssignmentDto assignmentDto,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting update of assignment {AssignmentId}", assignmentId);

        var assignment = await ExecuteTransactionAsync(
            async () => await mediator.Send(new GetAssignmentByIdQuery(chapterId, assignmentId), cancellationToken),
            cancellationToken: cancellationToken);

        if (assignment == null)
        {
            _logger.LogWarning("Assignment {AssignmentId} not found", assignmentId);
            throw new ArgumentException("Assignment does not exist.");
        }

        assignment.PatchUpdate(
            assignmentDto.Title,
            assignmentDto.Description,
            assignmentDto.RepositoryName,
            assignmentDto.RepositoryBaseBranchName,
            assignmentDto.RepositoryOwner,
            assignmentDto.RepositoryUrl,
            assignmentDto.Deadline,
            assignmentDto.MaxScore,
            assignmentDto.MaxAttemptsAmount,
            assignmentDto.Position,
            new ScoreSection(
                assignmentDto.AttemptCompilationSectionEnable ?? assignment.CompilationSection.IsEnabled,
                assignmentDto.AttemptCompilationMaxScore ?? assignment.CompilationSection.MaxScore,
                assignmentDto.AttemptCompilationMinScore ?? assignment.CompilationSection.MinScore
            ),
            qualitySection: new ScoreSection(
                assignmentDto.AttemptQualitySectionEnable ?? assignment.QualitySection.IsEnabled,
                assignmentDto.AttemptQualityMaxScore ?? assignment.QualitySection.MaxScore,
                assignmentDto.AttemptQualityMinScore ?? assignment.QualitySection.MinScore
            ),
            testsSection: new ScoreSection(
                assignmentDto.AttemptTestsSectionEnable ?? assignment.TestsSection.IsEnabled,
                assignmentDto.AttemptTestsMaxScore ?? assignment.TestsSection.MaxScore,
                assignmentDto.AttemptTestsMinScore ?? assignment.TestsSection.MinScore
            )
        );

        var updatedAssignment = await ExecuteTransactionAsync(
            async () => await mediator.Send(new UpdateAssignmentCommand(assignmentId, assignment), cancellationToken),
            cancellationToken: cancellationToken);

        _logger.LogInformation("Successfully updated assignment {AssignmentId}", assignmentId);
        return mapper.Map<RespondAssignmentDto>(updatedAssignment);
    }

    public async Task DeleteAssignmentAsync(Guid userId, Guid courseId, Guid chapterId, Guid assignmentId,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting deletion of assignment {AssignmentId}", assignmentId);

        await ExecuteTransactionAsync(
            async () => await mediator.Send(new DeleteAssignmentCommand(assignmentId), cancellationToken),
            cancellationToken: cancellationToken);

        var isAnyPublishedAssignmentInChapter = await ExecuteTransactionAsync(
            async () => await mediator.Send(new IsAnyPublishedAssignmentByChapterIdQuery(chapterId), cancellationToken),
            cancellationToken: cancellationToken);

        if (!isAnyPublishedAssignmentInChapter)
            await chapterService.UnpublishChapterAsync(userId, courseId, chapterId, cancellationToken);

        _logger.LogInformation("Successfully deleted assignment {AssignmentId}", assignmentId);
    }

    public async Task<RespondAssignmentDto> PublishAssignmentAsync(Guid userId, Guid chapterId, Guid assignmentId,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting publishing of assignment {AssignmentId}", assignmentId);

        var assignment = await ExecuteTransactionAsync(
            async () => await mediator.Send(new GetAssignmentByIdQuery(chapterId, assignmentId), cancellationToken),
            cancellationToken: cancellationToken);

        if (assignment == null)
        {
            _logger.LogWarning("Assignment {AssignmentId} not found", assignmentId);
            throw new ArgumentException("Assignment does not exist.");
        }

        assignment.Publish();

        var updatedAssignment = await ExecuteTransactionAsync(
            async () => await mediator.Send(new UpdateAssignmentCommand(assignmentId, assignment), cancellationToken),
            cancellationToken: cancellationToken);

        _logger.LogInformation("Successfully published assignment {AssignmentId}", assignmentId);
        return mapper.Map<RespondAssignmentDto>(updatedAssignment);
    }

    public async Task<RespondAssignmentDto> UnpublishAssignmentAsync(Guid userId, Guid courseId, Guid chapterId,
        Guid assignmentId,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting unpublishing of assignment {AssignmentId}", assignmentId);

        var assignment = await ExecuteTransactionAsync(
            async () => await mediator.Send(new GetAssignmentByIdQuery(chapterId, assignmentId), cancellationToken),
            cancellationToken: cancellationToken);

        if (assignment == null)
        {
            _logger.LogWarning("Assignment {AssignmentId} not found", assignmentId);
            throw new ArgumentException("Assignment does not exist.");
        }

        assignment.Unpublish();

        var updatedAssignment = await ExecuteTransactionAsync(
            async () => await mediator.Send(new UpdateAssignmentCommand(assignmentId, assignment), cancellationToken),
            cancellationToken: cancellationToken);

        var isAnyPublishedAssignmentInChapter = await ExecuteTransactionAsync(
            async () => await mediator.Send(new IsAnyPublishedAssignmentByChapterIdQuery(chapterId), cancellationToken),
            cancellationToken: cancellationToken);

        if (!isAnyPublishedAssignmentInChapter)
            await chapterService.UnpublishChapterAsync(userId, courseId, chapterId, cancellationToken);

        _logger.LogInformation("Successfully unpublished assignment {AssignmentId}", assignmentId);
        return mapper.Map<RespondAssignmentDto>(updatedAssignment);
    }

    public async Task ReorderAssignmentAsync(Guid userId, Guid courseId, Guid chapterId,
        IEnumerable<RequestReorderAssignmentDto> assignmentDtos, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting reordering of assignments in Chapter {ChapterId}", chapterId);

        await ExecuteTransactionAsync(
            async () =>
            {
                foreach (var assignment in assignmentDtos)
                    await mediator.Send(new UpdatePartialAssignmentCommand(assignment.Id, assignment.Position),
                        cancellationToken);
            },
            cancellationToken: cancellationToken);

        _logger.LogInformation("Successfully reordered assignments in Chapter {ChapterId}", chapterId);
    }

    public async Task<IReadOnlyList<RespondAssignmentDto>> GetAssignmentsAsync(
        Guid chapterId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting retrieval of all assignments in Chapter {ChapterId}", chapterId);

        var query = new GetAllAssignmentsByChapterIdQuery(chapterId);
        var result = await ExecuteWithExceptionHandlingAsync(
            async () => await mediator.Send(query, cancellationToken)
        );

        _logger.LogInformation("Successfully retrieved all assignments in Chapter {ChapterId}", chapterId);
        return result.Select(mapper.Map<RespondAssignmentDto>).ToList();
    }

    public async Task<RespondAssignmentDto?> GetAssignmentByIdAsync(Guid chapterId, Guid assignmentId,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting retrieval of assignment {AssignmentId}", assignmentId);

        var result = await ExecuteWithExceptionHandlingAsync(
            async () => await mediator.Send(new GetAssignmentByIdQuery(chapterId, assignmentId), cancellationToken)
        );

        if (result == null)
        {
            _logger.LogWarning("Assignment {AssignmentId} not found", assignmentId);
            return null;
        }

        _logger.LogInformation("Successfully retrieved assignment {AssignmentId}", assignmentId);
        return mapper.Map<RespondAssignmentDto>(result);
    }

    public async Task<RespondAssignmentAnalyticsDto?> GetAssignmentAnalyticsAsync(
        Guid chapterId,
        Guid assignmentId,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting retrieval of analytics for assignment {AssignmentId}", assignmentId);

        var attempts = await mediator.Send(new GetAllAttemptsByAssignmentIdQuery(assignmentId), cancellationToken);
        var attemptList = attempts.ToList();

        if (attemptList.Count == 0)
        {
            _logger.LogWarning("No analytics found for assignment {AssignmentId}", assignmentId);
            return null;
        }

        var attemptAnalyticsTasks = attemptList.Select(async attempt => new RespondAttemptAnalyticsDto
        {
            StudentFullName = await mediator.Send(new GetUserFullnameQuery(attempt.UserId), cancellationToken),
            CompilationScore = attempt.CompilationScore,
            QualityScore = attempt.QualityScore,
            TestsScore = attempt.TestsScore,
            FinalScore = attempt.FinalScore
        });

        var attemptAnalytics = await Task.WhenAll(attemptAnalyticsTasks);
        var assignment = await mediator.Send(new GetAssignmentByIdQuery(chapterId, assignmentId), cancellationToken);
        var attemptFinalResults = attemptList.Select(a => a.FinalScore).ToList();

        var passedCoefficient = assignment!.CompilationSection.MinScore +
                                assignment.QualitySection.MinScore +
                                assignment.TestsSection.MinScore;

        var result = new RespondAssignmentAnalyticsDto
        {
            Attempts = attemptAnalytics.ToList(),
            AssignmentTitle = assignment.Title,
            AverageScore = attemptFinalResults.Average(Convert.ToDouble),
            HighestScore = attemptFinalResults.Max(),
            SuccessCoefficient = (ushort)Math.Round(
                (double)attemptFinalResults.Count(score => score >= passedCoefficient) / attemptFinalResults.Count * 100
            )
        };

        _logger.LogInformation("Successfully retrieved analytics for assignment {AssignmentId}", assignmentId);
        return result;
    }
}