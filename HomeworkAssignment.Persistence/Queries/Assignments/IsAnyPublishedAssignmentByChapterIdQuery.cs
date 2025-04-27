using MediatR;

namespace HomeAssignment.Persistence.Queries.Assignments;

public record IsAnyPublishedAssignmentByChapterIdQuery(Guid ChapterId) : IRequest<bool>;