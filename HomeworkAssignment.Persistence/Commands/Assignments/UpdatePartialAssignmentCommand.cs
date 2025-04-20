using MediatR;

namespace HomeAssignment.Persistence.Commands.Assignments;

public sealed record UpdatePartialAssignmentCommand(Guid Id, ushort Position) : IRequest;