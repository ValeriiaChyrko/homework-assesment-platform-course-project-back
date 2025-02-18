using HomeAssignment.DTOs.RequestDTOs;
using HomeAssignment.DTOs.RespondDTOs;
using HomeAssignment.DTOs.SharedDTOs;
using HomeAssignment.Persistence.Commands.Assignments;
using HomeAssignment.Persistence.Queries.Assignments;
using HomeworkAssignment.Application.Abstractions;
using HomeworkAssignment.Application.Abstractions.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HomeworkAssignment.Application.Implementations;

public class AssignmentService : BaseService<AssignmentService>, IAssignmentService
{
    private readonly ILogger<AssignmentService> _logger;
    private readonly IMediator _mediator;

    public AssignmentService(IDatabaseTransactionManager transactionManager, IMediator mediator,
        ILogger<AssignmentService> logger)
        : base(logger, transactionManager)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<RespondAssignmentDto> CreateAssignmentAsync(RequestAssignmentDto assignmentDto,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Started creating assignment: {@AssignmentDto}", assignmentDto);
        var result = await ExecuteTransactionAsync(
            async () => await _mediator.Send(new CreateAssignmentCommand(assignmentDto), cancellationToken),
            cancellationToken: cancellationToken);
        _logger.LogInformation("Successfully created assignment with ID: {AssignmentId}", result.Id);
        return result;
    }

    public async Task<RespondAssignmentDto> UpdateAssignmentAsync(Guid id, RequestAssignmentDto assignmentDto,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Started updating assignment with ID: {AssignmentId}", id);
        var result = await ExecuteTransactionAsync(
            async () => await _mediator.Send(new UpdateAssignmentCommand(id, assignmentDto), cancellationToken),
            cancellationToken: cancellationToken);
        _logger.LogInformation("Successfully updated assignment with ID: {AssignmentId}", id);
        return result;
    }

    public async Task DeleteAssignmentAsync(Guid id, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Started deleting assignment with ID: {AssignmentId}", id);
        await ExecuteTransactionAsync(
            async () => await _mediator.Send(new DeleteAssignmentCommand(id), cancellationToken),
            cancellationToken: cancellationToken);
        _logger.LogInformation("Successfully deleted assignment with ID: {AssignmentId}", id);
    }

    public async Task<RespondAssignmentDto?> GetAssignmentByIdAsync(Guid id,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Started retrieving assignment by ID: {AssignmentId}", id);
        var result = await ExecuteWithExceptionHandlingAsync(
            async () => await _mediator.Send(new GetAssignmentByIdQuery(id), cancellationToken)
        );

        if (result != null)
            _logger.LogInformation("Successfully retrieved assignment with ID: {AssignmentId}", id);
        else
            _logger.LogWarning("No assignment found with ID: {AssignmentId}", id);

        return result;
    }

    public async Task<PagedList<RespondAssignmentDto>> GetAssignmentsAsync(
        RequestAssignmentFilterParameters filterParameters,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Started retrieving all assignments");

        var query = new GetAllAssignmentsQuery(filterParameters);

        var result = await ExecuteWithExceptionHandlingAsync(
            async () => await _mediator.Send(query, cancellationToken)
        );

        _logger.LogInformation("Successfully retrieved all assignments");
        return result;
    }
}