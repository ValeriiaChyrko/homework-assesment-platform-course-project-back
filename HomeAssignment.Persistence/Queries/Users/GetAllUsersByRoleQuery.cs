using HomeAssignment.Domain.Abstractions.Enums;
using HomeAssignment.DTOs.SharedDTOs;
using MediatR;

namespace HomeAssignment.Persistence.Queries.Users;

public record GetAllUsersByRoleQuery(UserRoles Role) : IRequest<IEnumerable<UserDto>>;