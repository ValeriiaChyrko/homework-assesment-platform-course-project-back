using HomeAssignment.DTOs.RespondDTOs;
using MediatR;

namespace HomeAssignment.Persistence.Queries.Students;

public record GetAllStudentsQuery : IRequest<IEnumerable<RespondStudentDto>>;