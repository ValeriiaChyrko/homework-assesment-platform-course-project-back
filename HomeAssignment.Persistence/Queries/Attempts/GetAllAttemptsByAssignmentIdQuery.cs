using HomeAssignment.DTOs.RespondDTOs;
using MediatR;

namespace HomeAssignment.Persistence.Queries.Attempts;

public record GetAllAttemptsByAssignmentIdQuery(Guid AssignmentId) : IRequest<IEnumerable<RespondAttemptDto>>;