using HomeAssignment.Domain.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Commands.AssignmentUserProgresses;

public sealed record UpdateAssignmentUserProgressCommand(AssignmentUserProgress AssignmentUserProgress)
    : IRequest<AssignmentUserProgress>;