using HomeAssignment.Domain.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Queries.UserChapterProgresses;

public record GetUserProgressByIdQuery(Guid UserId, Guid ChapterId)
    : IRequest<ChapterUserProgress?>;