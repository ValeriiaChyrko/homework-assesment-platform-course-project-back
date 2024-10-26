using HomeAssignment.Database.Contexts.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Commands.Students;

public sealed class DeleteStudentCommandHandler : IRequestHandler<DeleteStudentCommand>
{
    private readonly IHomeworkAssignmentDbContext _context;

    public DeleteStudentCommandHandler(IHomeworkAssignmentDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task Handle(DeleteStudentCommand command, CancellationToken cancellationToken)
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