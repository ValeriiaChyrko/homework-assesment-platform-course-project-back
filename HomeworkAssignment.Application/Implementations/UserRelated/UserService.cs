using AutoMapper;
using HomeAssignment.Domain.Abstractions;
using HomeAssignment.DTOs.SharedDTOs;
using HomeAssignment.Persistence.Commands.UserRoles;
using HomeAssignment.Persistence.Commands.Users;
using HomeAssignment.Persistence.Queries.Users;
using HomeworkAssignment.Application.Abstractions;
using HomeworkAssignment.Application.Abstractions.Contracts;
using HomeworkAssignment.Application.Abstractions.UserRelated;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HomeworkAssignment.Application.Implementations.UserRelated;

public class UserService(
    ILogger<UserService> logger,
    IDatabaseTransactionManager transactionManager,
    IMediator mediator,
    IMapper mapper)
    : BaseService<UserService>(logger, transactionManager), IUserService
{
    private readonly ILogger<UserService> _logger = logger;

    public async Task CreateOrUpdateUserAcync(UserDto userDto, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Started creating or updating user.");

        var user = mapper.Map<User>(userDto);
        var existingUser = await mediator.Send(new GetUserByIdQuery(user.Id), cancellationToken);

        if (existingUser is null)
        {
            await CreateUserWithRoles(user, userDto.Roles, cancellationToken);
            _logger.LogInformation("User  created successfully.");
        }
        else
        {
            await UpdateUserWithRoles(existingUser, userDto, cancellationToken);
            _logger.LogInformation("User  updated successfully.");
        }
    }

    private async Task CreateUserWithRoles(User user, IEnumerable<string> roles, CancellationToken cancellationToken)
    {
        await ExecuteTransactionAsync(
            async () =>
            {
                await mediator.Send(new CreateUserCommand(user), cancellationToken);
                await AssignRoles(user.Id, roles, cancellationToken);
            },
            cancellationToken: cancellationToken);
    }

    private async Task UpdateUserWithRoles(User existingUser, UserDto userDto, CancellationToken cancellationToken)
    {
        existingUser.PatchUpdate(userDto.FullName, userDto.Email, userDto.GithubUsername, userDto.GithubProfileUrl,
            userDto.GithubPictureUrl);

        await ExecuteTransactionAsync(
            async () =>
            {
                await mediator.Send(new UpdateUserCommand(existingUser), cancellationToken);
                await mediator.Send(new DeleteAllUserRolesCommand(existingUser.Id), cancellationToken);
                await AssignRoles(existingUser.Id, userDto.Roles, cancellationToken);
            },
            cancellationToken: cancellationToken);
    }

    private async Task AssignRoles(Guid userId, IEnumerable<string> roles, CancellationToken cancellationToken)
    {
        foreach (var roleName in roles.Distinct())
        {
            var roleId = await mediator.Send(new GetRoleIdByNameQuery(roleName), cancellationToken);
            await mediator.Send(new CreateUserRoleCommand(userId, roleId), cancellationToken);
            _logger.LogInformation("Assigned role '{RoleName}' to user.", roleName);
        }
    }
}