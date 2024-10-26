using HomeAssignment.DTOs.SharedDTOs;
using MediatR;

namespace HomeAssignment.Persistence.Queries.GitHubProfiles;

public record GetGitHubProfileByIdQuery(Guid Id) : IRequest<GitHubProfileDto?>;