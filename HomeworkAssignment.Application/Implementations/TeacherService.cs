using AutoMapper;
using HomeAssignment.Domain.Abstractions;
using HomeAssignment.Domain.Abstractions.Enums;
using HomeAssignment.DTOs.RequestDTOs;
using HomeAssignment.DTOs.RespondDTOs;
using HomeAssignment.DTOs.SharedDTOs;
using HomeAssignment.Persistence.Commands.GitHubProfiles;
using HomeAssignment.Persistence.Commands.Users;
using HomeAssignment.Persistence.Queries.GitHubProfiles;
using HomeAssignment.Persistence.Queries.Users;
using HomeworkAssignment.Application.Abstractions;
using HomeworkAssignment.Application.Abstractions.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HomeworkAssignment.Application.Implementations;

public class TeacherService : BaseService<TeacherService>, ITeacherService
{
    private readonly ILogger<TeacherService> _logger;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly IPasswordHasher _passwordHasher;

    public TeacherService(
        IDatabaseTransactionManager transactionManager,
        IMediator mediator,
        IPasswordHasher passwordHasher,
        ILogger<TeacherService> logger,
        IMapper mapper)
        : base(logger, transactionManager)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mapper = mapper;
    }

    public async Task<RespondTeacherDto> CreateTeacherAsync(RequestTeacherDto teacherDto,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Started creating teacher: {@TeacherDto}", teacherDto);
        var result = await ExecuteTransactionAsync(async () =>
        {
            var passwordHash = _passwordHasher.HashPassword(teacherDto.Password);
            var teacher = Teacher.Create(
                teacherDto.FullName,
                teacherDto.Email,
                passwordHash,
                teacherDto.GithubUsername,
                teacherDto.GithubProfileUrl,
                teacherDto.GithubPictureUrl
            );

            await _mediator.Send(new CreateUserCommand(_mapper.Map<UserDto>(teacher)), cancellationToken);
            await _mediator.Send(new CreateGitHubProfileCommand(_mapper.Map<GitHubProfileDto>(teacher)),
                cancellationToken);

            return _mapper.Map<RespondTeacherDto>(teacher);
        }, cancellationToken: cancellationToken);

        _logger.LogInformation("Successfully created teacher with ID: {TeacherId}", result.UserId);
        return result;
    }

    public async Task<RespondTeacherDto> UpdateTeacherAsync(Guid userId, Guid githubProfileId,
        RequestTeacherDto teacherDto, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(
            "Started updating teacher with User ID: {User Id} and GitHub Profile ID: {GithubProfileId}", userId,
            githubProfileId);
        var result = await ExecuteTransactionAsync(async () =>
        {
            var passwordHash = _passwordHasher.HashPassword(teacherDto.Password);
            var teacher = Teacher.Create(
                teacherDto.FullName,
                teacherDto.Email,
                passwordHash,
                teacherDto.GithubUsername,
                teacherDto.GithubProfileUrl,
                teacherDto.GithubPictureUrl
            );

            teacher.UserId = userId;
            teacher.GitHubProfileId = githubProfileId;

            await _mediator.Send(new UpdateUserCommand(_mapper.Map<UserDto>(teacher)), cancellationToken);
            await _mediator.Send(new UpdateGitHubProfileCommand(_mapper.Map<GitHubProfileDto>(teacher)),
                cancellationToken);

            return _mapper.Map<RespondTeacherDto>(teacher);
        }, cancellationToken: cancellationToken);

        _logger.LogInformation("Successfully updated teacher with User ID: {User Id}", userId);
        return result;
    }

    public async Task DeleteTeacherAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Started deleting teacher with User ID: {User Id}", userId);
        await ExecuteTransactionAsync(
            async () => { await _mediator.Send(new DeleteUserCommand(userId), cancellationToken); },
            cancellationToken: cancellationToken);
        _logger.LogInformation("Successfully deleted teacher with User ID: {User Id}", userId);
    }

    public async Task<RespondTeacherDto?> GetTeacherByIdAsync(Guid githubProfileId,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Started retrieving teacher by GitHub Profile ID: {GithubProfileId}", githubProfileId);
        var result = await ExecuteWithExceptionHandlingAsync(async () =>
        {
            var userDto = await _mediator.Send(new GetUserByGithubProfileIdQuery(githubProfileId), cancellationToken);
            if (userDto == null) return null;

            var gitHubProfileDto =
                await _mediator.Send(new GetGitHubProfileByIdQuery(githubProfileId), cancellationToken);
            var teacherDto = _mapper.Map<RespondTeacherDto>(userDto);

            if (gitHubProfileDto == null) return teacherDto;

            teacherDto.GitHubProfileId = gitHubProfileDto.Id;
            teacherDto.GithubUsername = gitHubProfileDto.GithubUsername;
            teacherDto.GithubProfileUrl = gitHubProfileDto.GithubProfileUrl;
            teacherDto.GithubPictureUrl = gitHubProfileDto.GithubPictureUrl;

            return teacherDto;
        });

        _logger.LogInformation("Successfully retrieved teacher with GitHub Profile ID: {GithubProfileId}",
            githubProfileId);
        return result;
    }

    public async Task<PagedList<RespondTeacherDto>> GetTeachersAsync(RequestUserFilterParameters filterParameters, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Started retrieving all teachers");
        var query = new GetAllUsersByRoleQuery(filterParameters, UserRoles.Teacher);
        var result = await ExecuteWithExceptionHandlingAsync(async () =>
        {
            var userDtos = await _mediator.Send(query, cancellationToken);
            var teachers = await Task.WhenAll(userDtos.Items.Select(async user =>
            {
                var teacherDto = _mapper.Map<RespondTeacherDto>(user);
                var gitHubProfiles =
                    await _mediator.Send(new GetAllGitHubProfilesByUserIdQuery(user.Id), cancellationToken);
                var mainGitHubProfile = gitHubProfiles?.FirstOrDefault();

                if (mainGitHubProfile == null) return teacherDto;

                teacherDto.GitHubProfileId = mainGitHubProfile.Id;
                teacherDto.GithubUsername = mainGitHubProfile.GithubUsername;
                teacherDto.GithubProfileUrl = mainGitHubProfile.GithubProfileUrl;
                teacherDto.GithubPictureUrl = mainGitHubProfile.GithubPictureUrl;

                return teacherDto;
            }));

            return new PagedList<RespondTeacherDto>(teachers.ToList(), userDtos.TotalCount, userDtos.Page, userDtos.PageSize);
        });

        _logger.LogInformation("Successfully retrieved all teachers");
        return result;
    }
}