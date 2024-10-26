using HomeAssignment.Domain.Abstractions.Contracts;
using HomeAssignment.DTOs.RequestDTOs;
using HomeAssignment.DTOs.RespondDTOs;
using HomeAssignment.Persistence.Commands.Attempts;
using HomeAssignment.Persistence.Queries.Attempts;
using HomeworkAssignment.Application.Abstractions.Contracts;
using MediatR;

namespace HomeworkAssignment.Application.Implementations;

public class AttemptService : IAttemptService
{
    private readonly ILogger _logger;
    private readonly IDatabaseTransactionManager _transactionManager;
    private readonly IMediator _mediator;

    public AttemptService(ILogger logger, IDatabaseTransactionManager transactionManager, IMediator mediator)
    {
        _logger = logger;
        _transactionManager = transactionManager;
        _mediator = mediator;
    }

    public async Task<RespondAttemptDto> CreateAttemptAsync(RequestAttemptDto attemptDto, CancellationToken cancellationToken = default)
    {
        await using var transaction = await _transactionManager.BeginTransactionAsync();
        try
        {
            var attempt = await _mediator.Send( new CreateAttemptCommand(attemptDto), cancellationToken);

            await _transactionManager.CommitAsync(transaction, cancellationToken);
            return attempt;
        }
        catch (Exception ex)
        {
            await _transactionManager.RollbackAsync(transaction, cancellationToken);
            _logger.Log($"Error creating attempt {ex.InnerException}. Using rollback transaction.");

            throw new Exception("Error creating attempt", ex);
        }
    }
    
    public async Task<RespondAttemptDto> UpdateAttemptAsync(Guid id, RequestAttemptDto attemptDto, CancellationToken cancellationToken = default)
    {
        await using var transaction = await _transactionManager.BeginTransactionAsync();
        try
        {
            var attempt = await _mediator.Send(new UpdateAttemptCommand(id, attemptDto), cancellationToken);

            await _transactionManager.CommitAsync(transaction, cancellationToken);
            return attempt;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            _logger.Log($"Error updating attempt {ex.InnerException}. Using rollback transaction.");

            throw new Exception("Error updating attempt", ex);
        }
    }
    
    public async Task DeleteAttemptAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await using var transaction = await _transactionManager.BeginTransactionAsync();
        try
        {
            await _mediator.Send(new DeleteAttemptCommand(id), cancellationToken);

            await _transactionManager.CommitAsync(transaction, cancellationToken);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            _logger.Log($"Error deleting attempt {ex.InnerException}. Using rollback transaction.");

            throw new Exception("Error deleting attempt", ex);
        }
    }
    
    public async Task<RespondAttemptDto?> GetAttemptByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var attempt = await _mediator.Send(new GetAttemptByIdQuery(id), cancellationToken);
            return attempt;
        }
        catch (Exception ex)
        {
            _logger.Log($"Error getting attempt {ex.InnerException}.");

            throw new Exception("Error getting attempt", ex);
        }
    }
    
    public async  Task<IReadOnlyList<RespondAttemptDto>> GetAttemptsByAssignmentIdAsync(Guid assignmentId, CancellationToken cancellationToken = default)
    {
        try
        {
            var attempts = await _mediator.Send(new GetAllAttemptsByAssignmentIdQuery(assignmentId), cancellationToken);
            return attempts.ToList();
        }
        catch (Exception ex)
        {
            _logger.Log($"Error getting attempts {ex.InnerException}.");

            throw new Exception("Error getting attempts", ex);
        }
    }
    
    public async Task<IReadOnlyList<RespondAttemptDto>> GetAttemptsByStudentIdAsync(Guid studentId, CancellationToken cancellationToken = default)
    {
        try
        {
            var attempts = await _mediator.Send(new GetAllAttemptsByStudentIdQuery(studentId), cancellationToken);
            return attempts.ToList();
        }
        catch (Exception ex)
        {
            _logger.Log($"Error getting attempts {ex.InnerException}.");

            throw new Exception("Error getting attempts", ex);
        }
    }
}