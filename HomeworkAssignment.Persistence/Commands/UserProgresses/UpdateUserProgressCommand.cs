using HomeAssignment.Domain.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Commands.UserProgresses;

public sealed record UpdateUserProgressCommand(UserProgress UserProgress) : IRequest<UserProgress>;