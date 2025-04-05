using MediatR;

namespace HomeAssignment.Persistence.Queries.UserChapterProgresses;

public record GetUserProgressCountQuery(Guid UserId, List<Guid> ChapterIds)
    : IRequest<int>;