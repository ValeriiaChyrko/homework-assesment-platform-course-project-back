using HomeAssignment.Domain.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Commands.UserProgresses;

public sealed record CreateUserProgressCommand(UserProgress UserProgress) : IRequest<UserProgress>;