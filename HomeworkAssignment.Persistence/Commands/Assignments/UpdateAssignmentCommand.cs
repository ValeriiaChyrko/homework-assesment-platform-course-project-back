using HomeAssignment.Domain.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Commands.Assignments;

public sealed record UpdateAssignmentCommand(Guid Id, Assignment Assignment)
    : IRequest<Assignment>;