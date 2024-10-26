using HomeAssignment.DTOs.RespondDTOs;
using MediatR;

namespace HomeAssignment.Persistence.Queries.Teachers;

public record GetAllTeachersQuery : IRequest<IEnumerable<RespondTeacherDto>>;