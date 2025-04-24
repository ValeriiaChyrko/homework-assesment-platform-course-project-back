using AutoMapper;
using HomeAssignment.Domain.Abstractions;
using HomeAssignment.DTOs.RequestDTOs.AttemptRelated;
using HomeAssignment.DTOs.RespondDTOs.AttemptRelated;
using HomeAssignment.Persistence.Commands.Attempts;
using HomeAssignment.Persistence.Queries.Attempts;
using HomeworkAssignment.Application.Abstractions;
using HomeworkAssignment.Application.Abstractions.AttemptRelated;
using HomeworkAssignment.Application.Abstractions.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HomeworkAssignment.Application.Implementations.AttemptRelated;

public class AttemptService(
    ILogger<AttemptService> logger,
    IDatabaseTransactionManager transactionManager,
    IMediator mediator,
    IMapper mapper)
    : BaseService<AttemptService>(logger, transactionManager), IAttemptService
{
    private readonly ILogger<AttemptService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<RespondAttemptDto> CreateAttemptAsync(Guid userId, Guid assignmentId,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Creating attempt for assignment {AssignmentId}", assignmentId);

        var lastAttempt = await ExecuteTransactionAsync(
            async () => await mediator.Send(new GetLastAttemptByIdQuery(userId, assignmentId), cancellationToken),
            cancellationToken: cancellationToken);

        var newPosition = (ushort)(lastAttempt?.Position + 1 ?? 1);
        var attempt = Attempt.Create(newPosition, userId, assignmentId);
        attempt.Position = newPosition;
        attempt.AssignmentId = assignmentId;

        attempt = await ExecuteTransactionAsync(
            async () => await mediator.Send(new CreateAttemptCommand(attempt), cancellationToken),
            cancellationToken: cancellationToken);

        _logger.LogInformation("Successfully created attempt {AttemptId}", attempt.Id);
        return mapper.Map<RespondAttemptDto>(attempt);
    }

    public async Task<RespondAttemptDto?> UpdateAttemptAsync(Guid userId, Guid assignmentId, Guid attemptId,
        RequestPartialAttemptDto attemptDto,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Updating attempt {AttemptId}", attemptId);

        var attempt = await ExecuteTransactionAsync(
            async () => await mediator.Send(new GetAttemptByIdQuery(assignmentId, attemptId), cancellationToken),
            cancellationToken: cancellationToken);

        if (attempt == null)
        {
            _logger.LogWarning("Attempt {AttemptId} not found", attemptId);
            return null;
        }

        attempt.UpdateBranchName(attemptDto.BranchName);

        var updatedAttempt = await ExecuteTransactionAsync(
            async () => await mediator.Send(new UpdateAttemptCommand(attemptId, attempt), cancellationToken),
            cancellationToken: cancellationToken);

        _logger.LogInformation("Successfully updated attempt {AttemptId}", updatedAttempt.Id);
        return mapper.Map<RespondAttemptDto>(updatedAttempt);
    }

    public async Task<RespondAttemptDto?> SubmitAttemptAsync(Guid userId, Guid assignmentId, Guid attemptId,
        RequestSubmitAttemptDto submitAttemptDto,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Submitting attempt {AttemptId}", attemptId);

        if (submitAttemptDto.Attempt.IsCompleted)
            await CreateAttemptAsync(userId, assignmentId, cancellationToken);

        return await UpdateExistingAttemptAsync(assignmentId, attemptId, submitAttemptDto, cancellationToken);
    }

    public async Task<IReadOnlyList<RespondAttemptDto>> GetAttemptsByAssignmentIdAsync(
        Guid userId, Guid assignmentId,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving all attempts for assignment {AssignmentId}", assignmentId);

        var result = await ExecuteWithExceptionHandlingAsync(
            async () => await mediator.Send(new GetAllAttemptsByUserIdQuery(userId, assignmentId), cancellationToken));

        return result.Select(mapper.Map<RespondAttemptDto>).ToList();
    }

    private async Task<RespondAttemptDto?> UpdateExistingAttemptAsync(Guid assignmentId, Guid attemptId,
        RequestSubmitAttemptDto submitAttemptDto,
        CancellationToken cancellationToken)
    {
        var attempt = await ExecuteTransactionAsync(
            async () => await mediator.Send(new GetAttemptByIdQuery(assignmentId, attemptId), cancellationToken),
            cancellationToken: cancellationToken);

        if (attempt == null)
        {
            _logger.LogWarning("Attempt {AttemptId} not found", attemptId);
            return null;
        }

        attempt.Submit(
            submitAttemptDto.Attempt.BranchName,
            submitAttemptDto.Attempt.CompilationScore,
            submitAttemptDto.Attempt.QualityScore,
            submitAttemptDto.Attempt.TestsScore
        );

        var updatedAttempt = await ExecuteTransactionAsync(
            async () => await mediator.Send(new UpdateAttemptCommand(attemptId, attempt), cancellationToken),
            cancellationToken: cancellationToken);

        _logger.LogInformation("Successfully updated attempt {AttemptId}", updatedAttempt.Id);
        return mapper.Map<RespondAttemptDto>(updatedAttempt);
    }
}