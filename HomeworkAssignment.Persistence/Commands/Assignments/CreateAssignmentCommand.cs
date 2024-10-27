using HomeAssignment.DTOs.RequestDTOs;
using HomeAssignment.DTOs.RespondDTOs;
using MediatR;

namespace HomeAssignment.Persistence.Commands.Assignments;

public sealed record CreateAssignmentCommand(RequestAssignmentDto AssignmentDto) : IRequest<RespondAssignmentDto>;