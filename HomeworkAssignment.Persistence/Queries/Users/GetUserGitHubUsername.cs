using HomeAssignment.Domain.Abstractions.Enums;
using MediatR;

namespace HomeAssignment.Persistence.Queries.Users;

public record GetUserGitHubUsername(Guid UserId) : IRequest<string>;