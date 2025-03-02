using MediatR;

namespace HomeAssignment.Persistence.Commands.UserProgresses;

public sealed record DeleteUserProgressCommand(Guid Id) : IRequest;