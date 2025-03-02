using HomeAssignment.Domain.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Commands.Users;

public sealed record UpdateUserCommand(User User) : IRequest<User>;