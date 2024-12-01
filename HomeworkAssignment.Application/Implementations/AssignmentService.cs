using HomeAssignment.Domain.Abstractions.Contracts;
using HomeAssignment.DTOs.RequestDTOs;
using HomeAssignment.DTOs.RespondDTOs;
using HomeAssignment.Persistence.Commands.Assignments;
using HomeAssignment.Persistence.Queries.Assignments;
using HomeworkAssignment.Application.Abstractions;
using HomeworkAssignment.Application.Abstractions.Contracts;
using MediatR;

namespace HomeworkAssignment.Application.Implementations;

public class AssignmentService : BaseService, IAssignmentService
{
    private readonly IMediator _mediator;

    public AssignmentService(ILogger logger, IDatabaseTransactionManager transactionManager, IMediator mediator)
        : base(logger, transactionManager)
    {
        _mediator = mediator;
    }

    public async Task<RespondAssignmentDto> CreateAssignmentAsync(RequestAssignmentDto assignmentDto,
        CancellationToken cancellationToken = default)
    {
        return await ExecuteWithTransactionAsync(
            async () => await _mediator.Send(new CreateAssignmentCommand(assignmentDto), cancellationToken),
            "creating assignment",
            cancellationToken
        );
    }

    public async Task<RespondAssignmentDto> UpdateAssignmentAsync(Guid id, RequestAssignmentDto assignmentDto,
        CancellationToken cancellationToken = default)
    {
        return await ExecuteWithTransactionAsync(
            async () => await _mediator.Send(new UpdateAssignmentCommand(id, assignmentDto), cancellationToken),
            "creating assignment",
            cancellationToken
        );
    }

    public async Task DeleteAssignmentAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await ExecuteWithTransactionAsync(
            async () => await _mediator.Send(new DeleteAssignmentCommand(id), cancellationToken),
            "deleting assignment",
            cancellationToken
        );
    }

    public async Task<RespondAssignmentDto?> GetAssignmentByIdAsync(Guid id,
        CancellationToken cancellationToken = default)
    {
        return await ExecuteWithExceptionHandlingAsync(
            async () => await _mediator.Send(new GetAssignmentByIdQuery(id), cancellationToken),
            "getting assignment by ID"
        );
    }

    public async Task<IReadOnlyList<RespondAssignmentDto>> GetAssignmentsAsync(
        CancellationToken cancellationToken = default)
    {
        return await ExecuteWithExceptionHandlingAsync(
            async () => (await _mediator.Send(new GetAllAssignmentsQuery(), cancellationToken)).ToList(),
            "getting all assignments"
        );
    }
}