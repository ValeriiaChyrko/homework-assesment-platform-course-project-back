using HomeAssignment.Database.Contexts.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Commands.UserProgresses;

public sealed class DeleteUserProgressCommandHandler : IRequestHandler<DeleteUserProgressCommand>
{
    private readonly IHomeworkAssignmentDbContext _context;

    public DeleteUserProgressCommandHandler(IHomeworkAssignmentDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task Handle(DeleteUserProgressCommand progressCommand, CancellationToken cancellationToken)
    {
        if (progressCommand is null) throw new ArgumentNullException(nameof(progressCommand));

        var userProgressEntity = await _context.UserProgressEntities.FindAsync(progressCommand.Id, cancellationToken);
        if (userProgressEntity != null) _context.UserProgressEntities.Remove(userProgressEntity);
    }
}