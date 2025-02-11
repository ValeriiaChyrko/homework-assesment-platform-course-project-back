using HomeAssignment.DTOs.RequestDTOs;
using HomeAssignment.DTOs.RespondDTOs;
using HomeAssignment.Persistence.Commands.Attempts;
using HomeAssignment.Persistence.Queries.Attempts;
using HomeworkAssignment.Application.Abstractions;
using HomeworkAssignment.Application.Abstractions.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HomeworkAssignment.Application.Implementations;

public class AttemptService : BaseService<AttemptService>, IAttemptService
{
    private readonly ILogger<AttemptService> _logger;
    private readonly IMediator _mediator;

    public AttemptService(IDatabaseTransactionManager transactionManager, IMediator mediator,
        ILogger<AttemptService> logger)
        : base(logger, transactionManager)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<RespondAttemptDto> CreateAttemptAsync(RequestAttemptDto attemptDto,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Started creating attempt: {@AttemptDto}", attemptDto);
        var result = await ExecuteTransactionAsync(
            async () => await _mediator.Send(new CreateAttemptCommand(attemptDto), cancellationToken),
            cancellationToken: cancellationToken);
        _logger.LogInformation("Successfully created attempt with ID: {AttemptId}", result.Id);
        return result;
    }

    public async Task<RespondAttemptDto> UpdateAttemptAsync(Guid id, RequestAttemptDto attemptDto,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Started updating attempt with ID: {AttemptId}", id);
        var result = await ExecuteTransactionAsync(
            async () => await _mediator.Send(new UpdateAttemptCommand(id, attemptDto), cancellationToken),
            cancellationToken: cancellationToken);
        _logger.LogInformation("Successfully updated attempt with ID: {AttemptId}", id);
        return result;
    }

    public async Task DeleteAttemptAsync(Guid id, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Started deleting attempt with ID: {AttemptId}", id);
        await ExecuteTransactionAsync(
            async () => await _mediator.Send(new DeleteAttemptCommand(id), cancellationToken),
            cancellationToken: cancellationToken);
        _logger.LogInformation("Successfully deleted attempt with ID: {AttemptId}", id);
    }

    public async Task<RespondAttemptDto?> GetAttemptByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Started retrieving attempt by ID: {AttemptId}", id);
        var result = await ExecuteWithExceptionHandlingAsync(
            async () => await _mediator.Send(new GetAttemptByIdQuery(id), cancellationToken)
        );

        if (result != null)
            _logger.LogInformation("Successfully retrieved attempt with ID: {AttemptId}", id);
        else
            _logger.LogWarning("No attempt found with ID: {AttemptId}", id);

        return result;
    }

    public async Task<IReadOnlyList<RespondAttemptDto>> GetAttemptsByAssignmentIdAsync(Guid assignmentId,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Started retrieving attempts by assignment ID: {AssignmentId}", assignmentId);
        var result = await ExecuteWithExceptionHandlingAsync(
            async () => (await _mediator.Send(new GetAllAttemptsByAssignmentIdQuery(assignmentId), cancellationToken))
                .ToList()
        );
        _logger.LogInformation("Successfully retrieved attempts for assignment ID: {AssignmentId}", assignmentId);
        return result;
    }

    public async Task<RespondAttemptDto> GetLastAttemptByAssignmentIdAsync(Guid assignmentId,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Started retrieving last attempt by assignment ID: {AssignmentId}", assignmentId);
        var result = await ExecuteWithExceptionHandlingAsync(
            async () => await _mediator.Send(new GetLastAttemptByAssignmentIdQuery(assignmentId), cancellationToken)
        );
        _logger.LogInformation("Successfully retrieved last attempt for assignment ID: {AssignmentId}", assignmentId);
        return result;
    }

    public async Task<IReadOnlyList<RespondAttemptDto>> GetStudentAttemptsAsync(Guid assignmentId, Guid studentId,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(
            "Started retrieving attempts for student ID: {StudentId} and assignment ID: {AssignmentId}", studentId,
            assignmentId);
        var result = await ExecuteWithExceptionHandlingAsync(
            async () => (await _mediator.Send(new GetAllAttemptsByStudentIdQuery(assignmentId, studentId),
                cancellationToken)).ToList()
        );
        _logger.LogInformation(
            "Successfully retrieved attempts for student ID: {StudentId} and assignment ID: {AssignmentId}", studentId,
            assignmentId);
        return result;
    }
}