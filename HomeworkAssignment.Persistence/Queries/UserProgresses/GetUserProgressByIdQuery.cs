using HomeAssignment.Domain.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Queries.UserProgresses;

public record GetUserProgressByIdQuery(Guid UserId, Guid ChapterId)
    : IRequest<UserProgress?>;