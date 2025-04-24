using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Database.Entities;
using MediatR;

namespace HomeAssignment.Persistence.Commands.UserRoles;

public sealed class CreateUserRoleCommandHandler(IHomeworkAssignmentDbContext context)
    : IRequestHandler<CreateUserRoleCommand>
{
    private readonly IHomeworkAssignmentDbContext
        _context = context ?? throw new ArgumentNullException(nameof(context));


    public async Task Handle(CreateUserRoleCommand roleCommand, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(roleCommand);

        var userEntity = new UserRolesEntity
        {
            UserId = roleCommand.UserId,
            RoleId = roleCommand.RoleId
        };

        await _context.UserRolesEntities.AddAsync(userEntity, cancellationToken);
    }
}