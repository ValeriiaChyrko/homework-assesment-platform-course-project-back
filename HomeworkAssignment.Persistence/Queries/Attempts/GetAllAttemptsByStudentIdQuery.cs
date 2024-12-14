using HomeAssignment.DTOs.RespondDTOs;
using MediatR;

namespace HomeAssignment.Persistence.Queries.Attempts;

public record GetAllAttemptsByStudentIdQuery(Guid AssignmentId, Guid StudentId)
    : IRequest<IEnumerable<RespondAttemptDto>>;