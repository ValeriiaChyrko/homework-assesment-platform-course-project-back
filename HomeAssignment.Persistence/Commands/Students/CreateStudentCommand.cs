using HomeAssignment.DTOs.RequestDTOs;
using HomeAssignment.DTOs.RespondDTOs;
using MediatR;

namespace HomeAssignment.Persistence.Commands.Students;

public sealed record CreateStudentCommand(RequestStudentDto StudentDto) : IRequest<RespondStudentDto>;