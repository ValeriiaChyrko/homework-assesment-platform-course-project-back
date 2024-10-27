using HomeAssignment.DTOs.SharedDTOs;
using MediatR;

namespace HomeAssignment.Persistence.Queries.GitHubProfiles;

public record GetAllGitHubProfilesByUserIdQuery(Guid UserId) : IRequest<IEnumerable<GitHubProfileDto>?>;