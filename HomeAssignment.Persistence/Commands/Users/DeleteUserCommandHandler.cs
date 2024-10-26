using HomeAssignment.Database.Contexts.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Commands.Users;

public sealed class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
{
    private readonly IHomeworkAssignmentDbContext _context;

    public DeleteUserCommandHandler(IHomeworkAssignmentDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task Handle(DeleteUserCommand command, CancellationToken cancellationToken)
    {
        if (command is null) throw new ArgumentNullException(nameof(command));

        var userEntity = await _context.UserEntities.FindAsync(command.Id, cancellationToken);
        if (userEntity != null)
        {
            _context.UserEntities.Remove(userEntity);
        }
    }
}