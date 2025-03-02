using MediatR;

namespace HomeAssignment.Persistence.Queries.UserProgresses;

public record GetUserProgressCountQuery(Guid UserId, List<Guid> ChapterIds)
    : IRequest<int>;