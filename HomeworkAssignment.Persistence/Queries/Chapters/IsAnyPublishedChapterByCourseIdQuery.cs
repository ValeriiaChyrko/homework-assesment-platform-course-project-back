using MediatR;

namespace HomeAssignment.Persistence.Queries.Chapters;

public record IsAnyPublishedChapterByCourseIdQuery(Guid CourseId) : IRequest<bool>;