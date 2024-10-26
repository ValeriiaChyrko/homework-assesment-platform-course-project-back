using HomeAssignment.DTOs.RespondDTOs;
using MediatR;

namespace HomeAssignment.Persistence.Queries.Assignments;

public record GetAllAssignmentsQuery : IRequest<IEnumerable<RespondAssignmentDto>>;