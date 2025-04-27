using HomeAssignment.Database.Contexts.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Commands.Assignments;

public sealed class DeleteAssignmentCommandHandler(IHomeworkAssignmentDbContext context)
    : IRequestHandler<DeleteAssignmentCommand>
{
    private readonly IHomeworkAssignmentDbContext
        _context = context ?? throw new ArgumentNullException(nameof(context));

    public async Task Handle(DeleteAssignmentCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        var assignmentEntity = await _context.AssignmentEntities.FindAsync(command.Id, cancellationToken);
        if (assignmentEntity != null) _context.AssignmentEntities.Remove(assignmentEntity);
    }
}