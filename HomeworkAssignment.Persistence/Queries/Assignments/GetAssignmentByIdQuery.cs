using HomeAssignment.Domain.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Queries.Assignments;

public record GetAssignmentByIdQuery(Guid ChapterId, Guid AssignmentId) : IRequest<Assignment?>;