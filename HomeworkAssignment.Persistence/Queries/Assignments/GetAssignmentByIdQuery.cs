using HomeAssignment.Domain.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Queries.Assignments;

public record GetAssignmentByIdQuery(Guid Id, Guid ChapterId) : IRequest<Assignment?>;