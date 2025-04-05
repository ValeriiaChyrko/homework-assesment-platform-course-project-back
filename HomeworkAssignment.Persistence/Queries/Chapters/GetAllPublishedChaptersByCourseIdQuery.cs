using HomeAssignment.Domain.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Queries.Chapters;

public record GetAllPublishedChaptersByCourseIdQuery(Guid CourseId) : IRequest<IEnumerable<Chapter>>;