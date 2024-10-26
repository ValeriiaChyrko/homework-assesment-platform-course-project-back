using MediatR;

namespace HomeAssignment.Persistence.Commands.Users;

public sealed record DeleteUserCommand(Guid Id) : IRequest;