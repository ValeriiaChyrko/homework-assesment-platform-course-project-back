using HomeAssignment.DTOs.SharedDTOs;
using MediatR;

namespace HomeAssignment.Persistence.Commands.Users;

public sealed record UpdateUserCommand(UserDto UserDto) : IRequest<UserDto>;