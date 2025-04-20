using HomeAssignment.Domain.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Queries.UserChapterProgresses;

public record GetUserChapterProgressByIdQuery(Guid UserId, Guid ChapterId)
    : IRequest<ChapterUserProgress?>;