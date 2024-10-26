using HomeAssignment.DTOs.SharedDTOs;
using MediatR;

namespace HomeAssignment.Persistence.Queries.Users;

public record GetUserByIdQuery(Guid Id) : IRequest<UserDto?>;