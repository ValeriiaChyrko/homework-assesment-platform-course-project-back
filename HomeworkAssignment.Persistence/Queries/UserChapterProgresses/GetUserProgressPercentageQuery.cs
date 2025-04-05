using MediatR;

namespace HomeAssignment.Persistence.Queries.UserChapterProgresses;

public record GetUserProgressPercentageQuery(Guid UserId, Guid CourseId)
    : IRequest<int>;