using HomeAssignment.DTOs.RequestDTOs;
using HomeAssignment.DTOs.RespondDTOs;
using MediatR;

namespace HomeAssignment.Persistence.Commands.Students;

public sealed record UpdateStudentCommand(Guid UserId, Guid GitHubProfileId, RequestStudentDto StudentDto) : IRequest<RespondStudentDto>;