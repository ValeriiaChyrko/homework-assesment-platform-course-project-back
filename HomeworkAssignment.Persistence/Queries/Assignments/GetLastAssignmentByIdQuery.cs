using HomeAssignment.Domain.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Queries.Assignments;

public record GetLastAssignmentByIdQuery(Guid ChapterId) : IRequest<Assignment?>;