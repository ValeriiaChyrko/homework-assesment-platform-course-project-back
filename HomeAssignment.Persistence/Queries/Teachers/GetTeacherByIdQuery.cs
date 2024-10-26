using HomeAssignment.DTOs.RespondDTOs;
using MediatR;

namespace HomeAssignment.Persistence.Queries.Teachers;

public record GetTeacherByIdQuery(Guid Id) : IRequest<RespondTeacherDto?>;