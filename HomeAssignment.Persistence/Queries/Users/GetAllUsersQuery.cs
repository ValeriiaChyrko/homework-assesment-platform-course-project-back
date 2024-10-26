using HomeAssignment.DTOs.RespondDTOs;
using HomeAssignment.DTOs.SharedDTOs;
using MediatR;

namespace HomeAssignment.Persistence.Queries.Users;

public record GetAllUsersQuery : IRequest<IEnumerable<UserDto>>;