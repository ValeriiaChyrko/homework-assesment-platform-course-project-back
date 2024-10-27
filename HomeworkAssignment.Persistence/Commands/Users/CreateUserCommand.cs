using HomeAssignment.DTOs.SharedDTOs;
using MediatR;

namespace HomeAssignment.Persistence.Commands.Users;

public sealed record CreateUserCommand(UserDto UserDto) : IRequest<UserDto>;