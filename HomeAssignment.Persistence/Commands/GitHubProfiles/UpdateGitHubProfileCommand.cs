using HomeAssignment.DTOs.SharedDTOs;
using MediatR;

namespace HomeAssignment.Persistence.Commands.GitHubProfiles;

public sealed record UpdateGitHubProfileCommand(GitHubProfileDto GitHubProfileDto) : IRequest<GitHubProfileDto>;