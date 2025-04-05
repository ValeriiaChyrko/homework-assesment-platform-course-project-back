using HomeAssignment.Database.Contexts.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Commands.AssignmentUserProgresses;

public sealed class DeleteAssignmentUserProgressCommandHandler : IRequestHandler<DeleteAssignmentUserProgressCommand>
{
    private readonly IHomeworkAssignmentDbContext _context;

    public DeleteAssignmentUserProgressCommandHandler(IHomeworkAssignmentDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task Handle(DeleteAssignmentUserProgressCommand progressCommand, CancellationToken cancellationToken)
    {
        if (progressCommand is null) throw new ArgumentNullException(nameof(progressCommand));

        var userProgressEntity = await _context.UserAssignmentProgressEntities.FindAsync(progressCommand.Id, cancellationToken);
        if (userProgressEntity != null) _context.UserAssignmentProgressEntities.Remove(userProgressEntity);
    }
}