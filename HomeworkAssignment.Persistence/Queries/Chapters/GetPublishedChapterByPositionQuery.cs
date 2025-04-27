using HomeAssignment.Domain.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Queries.Chapters;

public record GetPublishedChapterByPositionQuery(ushort Position, Guid CourseId) : IRequest<Chapter?>;