using MediatR;

namespace HomeAssignment.Persistence.Commands.Users;

public sealed record DeleteAllUserRolesCommand(Guid UserId) : IRequest;