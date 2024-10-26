using HomeAssignment.DTOs.RespondDTOs;
using MediatR;

namespace HomeAssignment.Persistence.Queries.Attempts;

public record GetAttemptByIdQuery(Guid Id) : IRequest<RespondAssignmentDto?>;