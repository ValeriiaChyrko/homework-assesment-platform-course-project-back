using HomeAssignment.DTOs.RequestDTOs;
using HomeAssignment.DTOs.RespondDTOs;
using MediatR;

namespace HomeAssignment.Persistence.Commands.Assignments;

public sealed record UpdateAssignmentCommand
    (Guid Id, RequestAssignmentDto AssignmentDto) : IRequest<RespondAssignmentDto>;