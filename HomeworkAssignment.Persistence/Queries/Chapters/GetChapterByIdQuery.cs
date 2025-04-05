using HomeAssignment.Domain.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Queries.Chapters;

public record GetChapterByIdQuery(Guid CourseId, Guid ChapterId) : IRequest<Chapter?>;