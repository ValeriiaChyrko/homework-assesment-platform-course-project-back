using HomeAssignment.Domain.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Commands.Attempts;

public sealed record CreateAttemptCommand(Attempt Attempt) : IRequest<Attempt>;