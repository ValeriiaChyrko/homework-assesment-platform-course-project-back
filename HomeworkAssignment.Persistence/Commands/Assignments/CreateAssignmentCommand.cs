using HomeAssignment.Domain.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Commands.Assignments;

public sealed record CreateAssignmentCommand(Assignment Assignment) : IRequest<Assignment>;