using HomeAssignment.DTOs.RequestDTOs;
using HomeAssignment.DTOs.RespondDTOs;
using MediatR;

namespace HomeAssignment.Persistence.Commands.Teachers;

public sealed record UpdateTeacherCommand(Guid UserId, Guid GitHubProfileId, RequestTeacherDto TeacherDto) : IRequest<RespondTeacherDto>;