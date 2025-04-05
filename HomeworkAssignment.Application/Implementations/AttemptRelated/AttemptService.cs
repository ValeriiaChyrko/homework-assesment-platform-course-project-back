using AutoMapper;
using HomeAssignment.Domain.Abstractions;
using HomeAssignment.DTOs.RequestDTOs;
using HomeAssignment.DTOs.RespondDTOs;
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
    
    public async Task<RespondAttemptDto> CreateAttemptAsync(Guid userId, Guid assignmentId, RequestAttemptDto attemptDto,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Started creating attempt: {@AttemptDto}", attemptDto);
        
        var lastAttempt = await ExecuteTransactionAsync(
            async () => await mediator.Send(new GetLastAttemptByIdQuery(userId, assignmentId), cancellationToken),
            cancellationToken:cancellationToken);
        
        var newPosition = lastAttempt?.Position + 1 ?? 1;
        var attempt = mapper.Map<Attempt>(attemptDto);
        attempt.Position = newPosition;
        attempt.AssignmentId = assignmentId;
        
        attempt = await ExecuteTransactionAsync(
            async () => await mediator.Send(new CreateAttemptCommand(attempt), cancellationToken),
            cancellationToken:cancellationToken);
        
        _logger.LogInformation("Successfully created attempt with ID: {AttemptId}", attempt.Id);
        return mapper.Map<RespondAttemptDto>(attempt);
    }

    public async Task<RespondAttemptDto?> UpdateAttemptAsync(Guid userId, Guid assignmentId, Guid attemptId, RequestPartialAttemptDto attemptDto,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Started updating attempt: {@AttemptDto}", attemptDto);
        
        var attempt = await ExecuteTransactionAsync(
            async () => await mediator.Send(new GetAttemptByIdQuery(assignmentId, attemptId), cancellationToken),
            cancellationToken:cancellationToken);

        if (attempt == null)
        {
            _logger.LogWarning("Attempt with ID: {AttemptId} not found", attemptId);
            return null;
        }
        
        attempt.UpdateBranchName(attemptDto.BranchName);
        
        var updatedAttempt = await ExecuteTransactionAsync(
            async () => await mediator.Send(new UpdateAttemptCommand(attemptId, attempt), cancellationToken),
            cancellationToken:cancellationToken);
        
        _logger.LogInformation("Successfully updated attempt with ID: {AttemptId}", updatedAttempt.Id);
        return mapper.Map<RespondAttemptDto>(updatedAttempt);
    }

    public async Task<RespondAttemptDto?> SubmitAttemptAsync(Guid userId, Guid assignmentId, Guid attemptId, RequestAttemptDto attemptDto,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Started submitting attempt: {@AttemptDto}", attemptDto);
        
        if (attemptDto.IsCompleted)
        {
            await CreateAttemptAsync(userId, assignmentId, attemptDto, cancellationToken);
        }

        return await UpdateExistingAttemptAsync(assignmentId, attemptId, attemptDto, cancellationToken);
    }

    private async Task<RespondAttemptDto?> UpdateExistingAttemptAsync(Guid assignmentId, Guid attemptId, RequestAttemptDto attemptDto,
        CancellationToken cancellationToken)
    {
        var attempt = await ExecuteTransactionAsync(
            async () => await mediator.Send(new GetAttemptByIdQuery(assignmentId, attemptId), cancellationToken),
            cancellationToken: cancellationToken);

        if (attempt == null)
        {
            _logger.LogWarning("Attempt with ID: {AttemptId} not found", attemptId);
            return null;
        }
        
        attempt.Submit(
            compilationScore: attemptDto.CompilationScore,
            qualityScore: attemptDto.QualityScore,
            testsScore: attemptDto.TestsScore
        );
        
        var updatedAttempt = await ExecuteTransactionAsync(
            async () => await mediator.Send(new UpdateAttemptCommand(attemptId, attempt), cancellationToken),
            cancellationToken: cancellationToken);
        
        _logger.LogInformation("Successfully updated attempt with ID: {AttemptId}", updatedAttempt.Id);
        return mapper.Map<RespondAttemptDto>(updatedAttempt);
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
}