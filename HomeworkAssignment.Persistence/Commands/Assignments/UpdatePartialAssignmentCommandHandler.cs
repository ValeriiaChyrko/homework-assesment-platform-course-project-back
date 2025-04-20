using HomeAssignment.Database.Contexts.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Commands.Assignments;

public sealed record UpdatePartialAssignmentCommandHandler : IRequestHandler<UpdatePartialAssignmentCommand>
{
    private readonly IHomeworkAssignmentDbContext _context;

    public UpdatePartialAssignmentCommandHandler(IHomeworkAssignmentDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task Handle(UpdatePartialAssignmentCommand command,
        CancellationToken cancellationToken = default)
    {
        if (command is null) throw new ArgumentNullException(nameof(command));

        var assignmentEntity = await _context.AssignmentEntities.FindAsync([command.Id], cancellationToken);

        if (assignmentEntity == null) throw new ArgumentNullException($"Assignment with ID {command.Id} not found.");

        assignmentEntity.Position = command.Position;
        assignmentEntity.UpdatedAt = DateTime.UtcNow;
    }
}