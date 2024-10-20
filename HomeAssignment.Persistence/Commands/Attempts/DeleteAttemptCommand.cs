using MediatR;

namespace HomeAssignment.Persistence.Commands.Attempts;

public sealed record DeleteAttemptCommand(Guid Id) : IRequest;