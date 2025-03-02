using HomeAssignment.Domain.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Commands.Users;

public sealed record CreateUserCommand(User User) : IRequest<User>;