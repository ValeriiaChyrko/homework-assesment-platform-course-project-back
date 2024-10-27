using MediatR;

namespace HomeAssignment.Persistence.Commands.Assignments;

public sealed record DeleteAssignmentCommand(Guid Id) : IRequest;