using HomeAssignment.Domain.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Commands.Assignments;

public sealed record UpdatePartialAssignmentCommand(Guid Id, int Position) : IRequest;