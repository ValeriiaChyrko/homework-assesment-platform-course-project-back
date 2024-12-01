using HomeAssignment.Domain.Abstractions.Contracts;
using HomeAssignment.DTOs.RequestDTOs;
using HomeAssignment.DTOs.RespondDTOs;
using HomeAssignment.Persistence.Commands.Attempts;
using HomeAssignment.Persistence.Queries.Attempts;
using HomeworkAssignment.Application.Abstractions;
using HomeworkAssignment.Application.Abstractions.Contracts;
using MediatR;

namespace HomeworkAssignment.Application.Implementations;

public class AttemptService : BaseService, IAttemptService
{
    private readonly IMediator _mediator;

    public AttemptService(ILogger logger, IDatabaseTransactionManager transactionManager, IMediator mediator)
        : base(logger, transactionManager)
    {
        _mediator = mediator;
    }

    public async Task<RespondAttemptDto> CreateAttemptAsync(RequestAttemptDto attemptDto,
        CancellationToken cancellationToken = default)
    {
        return await ExecuteWithTransactionAsync(
            async () => await _mediator.Send(new CreateAttemptCommand(attemptDto), cancellationToken),
            "creating attempt",
            cancellationToken
        );
    }

    public async Task<RespondAttemptDto> UpdateAttemptAsync(Guid id, RequestAttemptDto attemptDto,
        CancellationToken cancellationToken = default)
    {
        return await ExecuteWithTransactionAsync(
            async () => await _mediator.Send(new UpdateAttemptCommand(id, attemptDto), cancellationToken),
            "updating attempt",
            cancellationToken
        );
    }

    public async Task DeleteAttemptAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await ExecuteWithTransactionAsync(
            async () =>
            {
                await _mediator.Send(new DeleteAttemptCommand(id), cancellationToken);
                return Task.CompletedTask;
            },
            "deleting attempt",
            cancellationToken
        );
    }

    public async Task<RespondAttemptDto?> GetAttemptByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await ExecuteWithExceptionHandlingAsync(
            async () => await _mediator.Send(new GetAttemptByIdQuery(id), cancellationToken),
            "getting attempt by ID"
        );
    }

    public async Task<IReadOnlyList<RespondAttemptDto>> GetAttemptsByAssignmentIdAsync(Guid assignmentId,
        CancellationToken cancellationToken = default)
    {
        return await ExecuteWithExceptionHandlingAsync(
            async () => (await _mediator.Send(new GetAllAttemptsByAssignmentIdQuery(assignmentId), cancellationToken)).ToList(),
            "getting attempts by assignment ID"
        );
    }

    public async Task<RespondAttemptDto> GetLastAttemptByAssignmentIdAsync(Guid assignmentId,
        CancellationToken cancellationToken = default)
    {
        return await ExecuteWithExceptionHandlingAsync(
            async () => await _mediator.Send(new GetLastAttemptByAssignmentIdQuery(assignmentId), cancellationToken),
            "getting last attempt by assignment ID"
        );
    }

    public async Task<IReadOnlyList<RespondAttemptDto>> GetAttemptsByStudentIdAsync(Guid studentId,
        CancellationToken cancellationToken = default)
    {
        return await ExecuteWithExceptionHandlingAsync(
            async () => (await _mediator.Send(new GetAllAttemptsByStudentIdQuery(studentId), cancellationToken)).ToList(),
            "getting attempts by student ID"
        );
    }
}
