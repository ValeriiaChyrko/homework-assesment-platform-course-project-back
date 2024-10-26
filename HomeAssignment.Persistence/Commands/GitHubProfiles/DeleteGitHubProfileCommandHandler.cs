using HomeAssignment.Database.Contexts.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Commands.GitHubProfiles;

public sealed class DeleteGitHubProfileCommandHandler : IRequestHandler<DeleteGitHubProfileCommand>
{
    private readonly IHomeworkAssignmentDbContext _context;

    public DeleteGitHubProfileCommandHandler(IHomeworkAssignmentDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task Handle(DeleteGitHubProfileCommand command, CancellationToken cancellationToken)
    {
        if (command is null) throw new ArgumentNullException(nameof(command));

        var gitHubProfileEntity = await _context.GitHubProfilesEntities.FindAsync(command.Id, cancellationToken);
        if (gitHubProfileEntity != null)
        {
            _context.GitHubProfilesEntities.Remove(gitHubProfileEntity);
        }
    }
}