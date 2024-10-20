using HomeAssignment.Database.Contexts.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Commands.Assignments;

public sealed class DeleteAssignmentCommandHandler : IRequestHandler<DeleteAssignmentCommand>
{
    private readonly IHomeworkAssignmentDbContext _context;

    public DeleteAssignmentCommandHandler(IHomeworkAssignmentDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task Handle(DeleteAssignmentCommand command, CancellationToken cancellationToken)
    {
        if (command is null) throw new ArgumentNullException(nameof(command));

        var assignmentEntity = await _context.AssignmentEntities.FindAsync(command.Id, cancellationToken);
        if (assignmentEntity != null)
        {
            _context.AssignmentEntities.Remove(assignmentEntity);
        }
    }
}