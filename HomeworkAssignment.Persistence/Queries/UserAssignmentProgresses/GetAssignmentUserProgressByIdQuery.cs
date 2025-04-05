using HomeAssignment.Domain.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Queries.UserAssignmentProgresses;

public record GetAssignmentUserProgressByIdQuery(Guid UserId, Guid AssignmentId)
    : IRequest<AssignmentUserProgress?>;