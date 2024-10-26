using HomeAssignment.DTOs.RespondDTOs;
using MediatR;

namespace HomeAssignment.Persistence.Queries.Students;

public record GetStudentByIdQuery(Guid Id) : IRequest<RespondStudentDto?>;