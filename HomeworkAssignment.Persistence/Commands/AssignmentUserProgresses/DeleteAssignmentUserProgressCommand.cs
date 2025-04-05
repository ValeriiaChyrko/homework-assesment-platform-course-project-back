using MediatR;

namespace HomeAssignment.Persistence.Commands.AssignmentUserProgresses;

public sealed record DeleteAssignmentUserProgressCommand(Guid Id) : IRequest;