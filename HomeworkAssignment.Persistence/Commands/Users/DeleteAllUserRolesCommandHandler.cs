using HomeAssignment.Database.Contexts.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Commands.Users;

public sealed class DeleteAllUserRolesCommandHandler(IHomeworkAssignmentDbContext context)
    : IRequestHandler<DeleteAllUserRolesCommand>
{
    private readonly IHomeworkAssignmentDbContext
        _context = context ?? throw new ArgumentNullException(nameof(context));

    public Task Handle(DeleteAllUserRolesCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        var userRoles = _context.UserRolesEntities.Where(r => r.UserId == command.UserId);

        _context.UserRolesEntities.RemoveRange(userRoles);
        return Task.CompletedTask;
    }
}