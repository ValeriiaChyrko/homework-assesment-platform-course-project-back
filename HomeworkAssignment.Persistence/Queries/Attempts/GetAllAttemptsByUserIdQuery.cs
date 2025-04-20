using HomeAssignment.Domain.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Queries.Attempts;

public record GetAllAttemptsByUserIdQuery(Guid UserId, Guid AssignmentId)
    : IRequest<IEnumerable<Attempt>>;