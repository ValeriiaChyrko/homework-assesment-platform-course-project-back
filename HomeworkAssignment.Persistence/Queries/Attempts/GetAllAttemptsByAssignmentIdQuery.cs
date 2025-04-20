using HomeAssignment.Domain.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Queries.Attempts;

public record GetAllAttemptsByAssignmentIdQuery(Guid AssignmentId)
    : IRequest<IEnumerable<Attempt>>;