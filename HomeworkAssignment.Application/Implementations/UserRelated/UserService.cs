using AutoMapper;
using HomeAssignment.Domain.Abstractions.Enums;
using HomeAssignment.Persistence.Queries.Users;
using HomeworkAssignment.Application.Abstractions;
using HomeworkAssignment.Application.Abstractions.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HomeworkAssignment.Application.Implementations.UserRelated;

public class UserService : BaseService<UserService>, IUserService
{
    private readonly ILogger<UserService> _logger;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public UserService(ILogger<UserService> logger, IDatabaseTransactionManager transactionManager, IMediator mediator,
        IMapper mapper) : base(logger, transactionManager)
    {
        _logger = logger;
        _mediator = mediator;
        _mapper = mapper;
    }

    public async Task<bool> CheckIfUserInTeacherRole(Guid userId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Started checking user with ID: {UserId}", userId);

        var isTeacher = await ExecuteTransactionAsync(
            async () => await _mediator.Send(new CheckIfUserInRoleQuery(userId, UserRoles.Teacher), cancellationToken),
            cancellationToken: cancellationToken);

        _logger.LogInformation("Finished checking user with ID: {UserId}", userId);
        return isTeacher;
    }

    public async Task<string> GetUserGitHubUsername(Guid userId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Started checking user with ID: {UserId}", userId);

        var username = await ExecuteTransactionAsync(
            async () => await _mediator.Send(new GetUserGitHubUsername(userId), cancellationToken),
            cancellationToken: cancellationToken);

        _logger.LogInformation("Finished checking user with ID: {UserId}", userId);
        return username;
    }
}