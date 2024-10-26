using HomeAssignment.Domain.Abstractions.Contracts;
using HomeAssignment.DTOs.RequestDTOs;
using HomeAssignment.DTOs.RespondDTOs;
using HomeAssignment.Persistence.Commands.Assignments;
using HomeAssignment.Persistence.Queries.Assignments;
using HomeworkAssignment.Application.Abstractions.Contracts;
using MediatR;

namespace HomeworkAssignment.Application.Implementations;

public class AssignmentService : IAssignmentService
{
    private readonly ILogger _logger;
    private readonly IDatabaseTransactionManager _transactionManager;
    private readonly IMediator _mediator;

    public AssignmentService(ILogger logger, IDatabaseTransactionManager transactionManager, IMediator mediator)
    {
        _logger = logger;
        _transactionManager = transactionManager;
        _mediator = mediator;
    }

    public async Task<RespondAssignmentDto> CreateAssignmentAsync(RequestAssignmentDto assignmentDto, CancellationToken cancellationToken = default)
    {
        await using var transaction = await _transactionManager.BeginTransactionAsync();
        try
        {
            var assignment = await _mediator.Send( new CreateAssignmentCommand(assignmentDto), cancellationToken);

            await _transactionManager.CommitAsync(transaction, cancellationToken);
            return assignment;
        }
        catch (Exception ex)
        {
            await _transactionManager.RollbackAsync(transaction, cancellationToken);
            _logger.Log($"Error creating assignment {ex.InnerException}. Using rollback transaction.");

            throw new Exception("Error creating assignment", ex);
        }
    }
    
    public async Task<RespondAssignmentDto> UpdateAssignmentAsync(Guid id, RequestAssignmentDto assignmentDto, 
        CancellationToken cancellationToken = default)
    {
        await using var transaction = await _transactionManager.BeginTransactionAsync();
        try
        {
            var assignment = await _mediator.Send(new UpdateAssignmentCommand(id, assignmentDto), cancellationToken);

            await _transactionManager.CommitAsync(transaction, cancellationToken);
            return assignment;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            _logger.Log($"Error updating assignment {ex.InnerException}. Using rollback transaction.");

            throw new Exception("Error updating assignment", ex);
        }
    }
    
    public async Task DeleteAssignmentAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await using var transaction = await _transactionManager.BeginTransactionAsync();
        try
        {
            await _mediator.Send(new DeleteAssignmentCommand(id), cancellationToken);

            await _transactionManager.CommitAsync(transaction, cancellationToken);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            _logger.Log($"Error deleting assignment {ex.InnerException}. Using rollback transaction.");

            throw new Exception("Error deleting assignment", ex);
        }
    }
    
    public async Task<RespondAssignmentDto?> GetAssignmentByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var assignment = await _mediator.Send(new GetAssignmentByIdQuery(id), cancellationToken);
            return assignment;
        }
        catch (Exception ex)
        {
            _logger.Log($"Error getting assignment {ex.InnerException}.");

            throw new Exception("Error getting assignment", ex);
        }
    }
    
    public async Task<IReadOnlyList<RespondAssignmentDto>> GetAssignmentsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var assignments = await _mediator.Send(new GetAllAssignmentsQuery(), cancellationToken);
            
            return assignments.ToList();
        }
        catch (Exception ex)
        {
            _logger.Log($"Error getting assignments entities {ex.InnerException}.");

            throw new Exception("Error getting assignments entities", ex);
        }
    }
}