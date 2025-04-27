using MediatR;

namespace HomeAssignment.Persistence.Commands.UserRoles;

public sealed record CreateUserRoleCommand(Guid UserId, int RoleId) : IRequest;