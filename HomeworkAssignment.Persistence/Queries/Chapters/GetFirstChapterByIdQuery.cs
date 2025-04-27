using HomeAssignment.Domain.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Queries.Chapters;

public record GetFirstChapterByIdQuery(Guid CourseId) : IRequest<Chapter?>;