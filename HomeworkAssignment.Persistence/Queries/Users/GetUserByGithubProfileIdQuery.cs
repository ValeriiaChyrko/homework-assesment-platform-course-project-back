using HomeAssignment.DTOs.SharedDTOs;
using MediatR;

namespace HomeAssignment.Persistence.Queries.Users;

public record GetUserByGithubProfileIdQuery(Guid GithubProfileId) : IRequest<UserDto?>;