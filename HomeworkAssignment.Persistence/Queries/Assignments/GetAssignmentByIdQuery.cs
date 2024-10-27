using HomeAssignment.DTOs.RespondDTOs;
using MediatR;

namespace HomeAssignment.Persistence.Queries.Assignments;

public record GetAssignmentByIdQuery(Guid Id) : IRequest<RespondAssignmentDto?>;