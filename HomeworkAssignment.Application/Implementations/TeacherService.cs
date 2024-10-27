using AutoMapper;
using HomeAssignment.Domain.Abstractions;
using HomeAssignment.Domain.Abstractions.Contracts;
using HomeAssignment.Domain.Abstractions.Enums;
using HomeAssignment.DTOs.RequestDTOs;
using HomeAssignment.DTOs.RespondDTOs;
using HomeAssignment.DTOs.SharedDTOs;
using HomeAssignment.Persistence.Commands.GitHubProfiles;
using HomeAssignment.Persistence.Commands.Users;
using HomeAssignment.Persistence.Queries.GitHubProfiles;
using HomeAssignment.Persistence.Queries.Users;
using HomeworkAssignment.Application.Abstractions.Contracts;
using MediatR;

namespace HomeworkAssignment.Application.Implementations;

public class TeacherService : ITeacherService
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IDatabaseTransactionManager _transactionManager;
    private readonly IMediator _mediator;

    public TeacherService(ILogger logger, IMapper mapper, IPasswordHasher passwordHasher,
        IDatabaseTransactionManager transactionManager, IMediator mediator)
    {
        _logger = logger;
        _mapper = mapper;
        _passwordHasher = passwordHasher;
        _transactionManager = transactionManager;
        _mediator = mediator;
    }

    public async Task<RespondTeacherDto> CreateTeacherAsync(RequestTeacherDto teacherDto,
        CancellationToken cancellationToken = default)
    {
        await using var transaction = await _transactionManager.BeginTransactionAsync();
        try
        {
            var passwordHash = _passwordHasher.HashPassword(teacherDto.Password);

            var teacher = Teacher.Create(
                teacherDto.FullName,
                teacherDto.Email,
                passwordHash,
                teacherDto.GithubUsername,
                teacherDto.GithubAccessToken,
                teacherDto.GithubProfileUrl,
                teacherDto.GithubPictureUrl
            );

            var userDto = _mapper.Map<UserDto>(teacher);
            await _mediator.Send(new CreateUserCommand(userDto), cancellationToken);

            var profileDto = _mapper.Map<GitHubProfileDto>(teacher);
            await _mediator.Send(new CreateGitHubProfileCommand(profileDto), cancellationToken);

            await _transactionManager.CommitAsync(transaction, cancellationToken);
            return _mapper.Map<RespondTeacherDto>(teacher);
        }
        catch (Exception ex)
        {
            await _transactionManager.RollbackAsync(transaction, cancellationToken);
            _logger.Log($"Error creating teacher {ex.InnerException}. Using rollback transaction.");

            throw new Exception("Error creating teacher", ex);
        }
    }

    public async Task<RespondTeacherDto> UpdateTeacherAsync(Guid userId, Guid githubProfileId,
        RequestTeacherDto teacherDto, CancellationToken cancellationToken = default)
    {
        await using var transaction = await _transactionManager.BeginTransactionAsync();
        try
        {
            var passwordHash = _passwordHasher.HashPassword(teacherDto.Password);

            var teacher = Teacher.Create(
                teacherDto.FullName,
                teacherDto.Email,
                passwordHash,
                teacherDto.GithubUsername,
                teacherDto.GithubAccessToken,
                teacherDto.GithubProfileUrl,
                teacherDto.GithubPictureUrl
            );
            teacher.UserId = userId;
            teacher.GitHubProfileId = githubProfileId;

            var userDto = _mapper.Map<UserDto>(teacher);
            await _mediator.Send(new UpdateUserCommand(userDto), cancellationToken);

            var profileDto = _mapper.Map<GitHubProfileDto>(teacher);
            await _mediator.Send(new UpdateGitHubProfileCommand(profileDto), cancellationToken);

            await _transactionManager.CommitAsync(transaction, cancellationToken);
            return _mapper.Map<RespondTeacherDto>(teacher);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            _logger.Log($"Error updating teacher {ex.InnerException}. Using rollback transaction.");

            throw new Exception("Error updating teacher", ex);
        }
    }

    public async Task DeleteTeacherAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        await using var transaction = await _transactionManager.BeginTransactionAsync();
        try
        {
            await _mediator.Send(new DeleteUserCommand(userId), cancellationToken);

            await _transactionManager.CommitAsync(transaction, cancellationToken);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            _logger.Log($"Error deleting teacher {ex.InnerException}. Using rollback transaction.");

            throw new Exception("Error getting teacher", ex);
        }
    }

    public async Task<RespondTeacherDto?> GetTeacherByIdAsync(Guid userId, Guid githubProfileId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var userDto = await _mediator.Send(new GetUserByIdQuery(userId), cancellationToken);
            if (userDto == null) return null;

            var gitHubProfileDto =
                await _mediator.Send(new GetGitHubProfileByIdQuery(githubProfileId), cancellationToken);

            var teacherWithProfileDto = _mapper.Map<RespondTeacherDto>(userDto);
            if (gitHubProfileDto == null) return teacherWithProfileDto;

            teacherWithProfileDto.GitHubProfileId = gitHubProfileDto.Id;
            teacherWithProfileDto.GithubUsername = gitHubProfileDto.GithubUsername;
            teacherWithProfileDto.GithubAccessToken = gitHubProfileDto.GithubAccessToken;
            teacherWithProfileDto.GithubProfileUrl = gitHubProfileDto.GithubProfileUrl;
            teacherWithProfileDto.GithubPictureUrl = gitHubProfileDto.GithubPictureUrl;

            return teacherWithProfileDto;
        }
        catch (Exception ex)
        {
            _logger.Log($"Error getting teacher {ex.InnerException}.");

            throw new Exception("Error getting teacher", ex);
        }
    }

    public async Task<IReadOnlyList<RespondTeacherDto>> GetTeacherAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var userDtos = await _mediator.Send(new GetAllUsersByRoleQuery(UserRoles.Teacher), cancellationToken);

            var teacherDtos = await Task.WhenAll(userDtos.Select(async user =>
            {
                var teacherWithProfileDto = _mapper.Map<RespondTeacherDto>(user);

                var gitHubProfiles =
                    await _mediator.Send(new GetAllGitHubProfilesByUserIdQuery(user.Id), cancellationToken);
                var mainGitHubProfile = gitHubProfiles?.FirstOrDefault();

                if (mainGitHubProfile != null)
                {
                    teacherWithProfileDto.GitHubProfileId = mainGitHubProfile.Id;
                    teacherWithProfileDto.GithubUsername = mainGitHubProfile.GithubUsername;
                    teacherWithProfileDto.GithubAccessToken = mainGitHubProfile.GithubAccessToken;
                    teacherWithProfileDto.GithubProfileUrl = mainGitHubProfile.GithubProfileUrl;
                    teacherWithProfileDto.GithubPictureUrl = mainGitHubProfile.GithubPictureUrl;
                }

                return teacherWithProfileDto;
            }));

            return teacherDtos.ToList();
        }
        catch (Exception ex)
        {
            _logger.Log($"Error getting teachers entities {ex.InnerException}.");
            throw new Exception("Error getting teachers entities", ex);
        }
    }
}