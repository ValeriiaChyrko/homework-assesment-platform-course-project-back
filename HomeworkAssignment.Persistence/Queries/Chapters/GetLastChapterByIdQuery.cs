using HomeAssignment.Domain.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Queries.Chapters;

public record GetLastChapterByIdQuery(Guid CourseId) : IRequest<Chapter?>;