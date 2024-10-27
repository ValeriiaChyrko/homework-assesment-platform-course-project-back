using HomeAssignment.DTOs.SharedDTOs;
using MediatR;

namespace HomeAssignment.Persistence.Commands.GitHubProfiles;

public sealed record CreateGitHubProfileCommand(GitHubProfileDto GitHubProfileDto) : IRequest<GitHubProfileDto>;