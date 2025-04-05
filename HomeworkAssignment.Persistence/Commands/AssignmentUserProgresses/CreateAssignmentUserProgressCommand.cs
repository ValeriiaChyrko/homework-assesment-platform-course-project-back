using HomeAssignment.Domain.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Commands.AssignmentUserProgresses;

public sealed record CreateAssignmentUserProgressCommand(AssignmentUserProgress AssignmentUserProgress) : IRequest<AssignmentUserProgress>;