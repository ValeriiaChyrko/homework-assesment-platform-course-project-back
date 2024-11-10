using HomeAssignment.DTOs.RespondDTOs;
using MediatR;

namespace HomeAssignment.Persistence.Queries.Attempts;

public record GetLastAttemptByAssignmentIdQuery(Guid AssignmentId) : IRequest<RespondAttemptDto>;