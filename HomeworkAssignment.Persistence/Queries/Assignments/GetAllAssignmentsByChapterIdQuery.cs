using HomeAssignment.Domain.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Queries.Assignments;

public record GetAllAssignmentsByChapterIdQuery(Guid ChapterId) : IRequest<IEnumerable<Assignment>>;