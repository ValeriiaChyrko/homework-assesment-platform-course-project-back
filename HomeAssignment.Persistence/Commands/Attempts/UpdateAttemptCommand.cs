using HomeAssignment.DTOs.RequestDTOs;
using HomeAssignment.DTOs.RespondDTOs;
using MediatR;

namespace HomeAssignment.Persistence.Commands.Attempts;

public sealed record UpdateAttemptCommand(Guid Id, RequestAttemptDto AttemptDto) : IRequest<RespondAttemptDto>;