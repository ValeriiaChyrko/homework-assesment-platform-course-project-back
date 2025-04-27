using HomeAssignment.Domain.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Queries.Attempts;

public record GetAttemptByIdQuery(Guid AssignmentId, Guid AttemptId)
    : IRequest<Attempt?>;