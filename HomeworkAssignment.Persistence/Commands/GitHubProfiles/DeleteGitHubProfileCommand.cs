using MediatR;

namespace HomeAssignment.Persistence.Commands.GitHubProfiles;

public sealed record DeleteGitHubProfileCommand(Guid Id) : IRequest;