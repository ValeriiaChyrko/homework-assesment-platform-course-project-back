using HomeAssignment.Domain.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Queries.Attempts;

public record GetAllAttemptsByUserIdQuery(Guid AssignmentId, Guid UserId)
    : IRequest<IEnumerable<Attempt>>;