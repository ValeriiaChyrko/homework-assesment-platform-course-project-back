using HomeAssignment.Domain.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Queries.Attempts;

public record GetLastAttemptByIdQuery(Guid UserId, Guid AssignmentId) : IRequest<Attempt?>;