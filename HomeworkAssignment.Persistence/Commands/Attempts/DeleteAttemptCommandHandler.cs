using HomeAssignment.Database.Contexts.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Commands.Attempts;

public sealed class DeleteAttemptCommandHandler : IRequestHandler<DeleteAttemptCommand>
{
    private readonly IHomeworkAssignmentDbContext _context;

    public DeleteAttemptCommandHandler(IHomeworkAssignmentDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task Handle(DeleteAttemptCommand command, CancellationToken cancellationToken)
    {
        if (command is null) throw new ArgumentNullException(nameof(command));

        var attemptEntity = await _context.AttemptEntities.FindAsync([command.Id], cancellationToken);
        if (attemptEntity != null) _context.AttemptEntities.Remove(attemptEntity);
    }
}