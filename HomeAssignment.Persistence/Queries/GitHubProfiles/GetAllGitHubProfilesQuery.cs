using HomeAssignment.DTOs.SharedDTOs;
using MediatR;

namespace HomeAssignment.Persistence.Queries.GitHubProfiles;

public record GetAllGitHubProfilesQuery : IRequest<IEnumerable<GitHubProfileDto>>;