using HomeAssignment.Domain.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Commands.Attempts;

public sealed record UpdateAttemptCommand(Guid Id, Attempt Attempt) : IRequest<Attempt>;