using HomeAssignment.Database.Contexts.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Commands.Teachers;

public sealed class DeleteTeacherCommandHandler : IRequestHandler<DeleteTeacherCommand>
{
    private readonly IHomeworkAssignmentDbContext _context;

    public DeleteTeacherCommandHandler(IHomeworkAssignmentDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task Handle(DeleteTeacherCommand command, CancellationToken cancellationToken)
    {
        if (command is null)
        {
            throw new ArgumentNullException(nameof(command));
        }

        var userEntity = await _context.UserEntities.FindAsync(command.UserId, cancellationToken);
        if (userEntity != null)
        {
            _context.UserEntities.Remove(userEntity);
        }
    }
}