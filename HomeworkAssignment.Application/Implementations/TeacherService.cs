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
using HomeworkAssignment.Application.Abstractions;
using HomeworkAssignment.Application.Abstractions.Contracts;
using MediatR;

namespace HomeworkAssignment.Application.Implementations;

public class TeacherService : BaseService, ITeacherService
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly IPasswordHasher _passwordHasher;

    public TeacherService(ILogger logger, IMapper mapper, IPasswordHasher passwordHasher, IDatabaseTransactionManager transactionManager, IMediator mediator)
        : base(logger, transactionManager)
    {
        _mapper = mapper;
        _passwordHasher = passwordHasher;
        _mediator = mediator;
    }

    public async Task<RespondTeacherDto> CreateTeacherAsync(RequestTeacherDto teacherDto, CancellationToken cancellationToken = default)
    {
        return await ExecuteWithTransactionAsync(async () =>
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
            await _mediator.Send(new CreateGitHubProfileCommand(_mapper.Map<GitHubProfileDto>(teacher)), cancellationToken);

            return _mapper.Map<RespondTeacherDto>(teacher);
        }, "creating teacher", cancellationToken);
    }

    public async Task<RespondTeacherDto> UpdateTeacherAsync(Guid userId, Guid githubProfileId, RequestTeacherDto teacherDto, CancellationToken cancellationToken = default)
    {
        return await ExecuteWithTransactionAsync(async () =>
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
            await _mediator.Send(new UpdateGitHubProfileCommand(_mapper.Map<GitHubProfileDto>(teacher)), cancellationToken);

            return _mapper.Map<RespondTeacherDto>(teacher);
        }, "updating teacher", cancellationToken);
    }

    public async Task DeleteTeacherAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        await ExecuteWithTransactionAsync(async () =>
        {
            await _mediator.Send(new DeleteUserCommand(userId), cancellationToken);
            return Task.CompletedTask;
        }, "deleting teacher", cancellationToken);
    }

    public async Task<RespondTeacherDto?> GetTeacherByIdAsync(Guid githubProfileId, CancellationToken cancellationToken = default)
    {
        return await ExecuteWithExceptionHandlingAsync(async () =>
        {
            var userDto = await _mediator.Send(new GetUserByGithubProfileIdQuery(githubProfileId), cancellationToken);
            if (userDto == null) return null;

            var gitHubProfileDto = await _mediator.Send(new GetGitHubProfileByIdQuery(githubProfileId), cancellationToken);

            var teacherDto = _mapper.Map<RespondTeacherDto>(userDto);
            if (gitHubProfileDto == null) return teacherDto;

            teacherDto.GitHubProfileId = gitHubProfileDto.Id;
            teacherDto.GithubUsername = gitHubProfileDto.GithubUsername;
            teacherDto.GithubProfileUrl = gitHubProfileDto.GithubProfileUrl;
            teacherDto.GithubPictureUrl = gitHubProfileDto.GithubPictureUrl;

            return teacherDto;
        }, "getting teacher by ID");
    }

    public async Task<IReadOnlyList<RespondTeacherDto>> GetTeachersAsync(CancellationToken cancellationToken = default)
    {
        return await ExecuteWithExceptionHandlingAsync(async () =>
        {
            var userDtos = await _mediator.Send(new GetAllUsersByRoleQuery(UserRoles.Teacher), cancellationToken);

            var teachers = await Task.WhenAll(userDtos.Select(async user =>
            {
                var teacherDto = _mapper.Map<RespondTeacherDto>(user);

                var gitHubProfiles = await _mediator.Send(new GetAllGitHubProfilesByUserIdQuery(user.Id), cancellationToken);
                var mainGitHubProfile = gitHubProfiles?.FirstOrDefault();

                if (mainGitHubProfile == null) return teacherDto;

                teacherDto.GitHubProfileId = mainGitHubProfile.Id;
                teacherDto.GithubUsername = mainGitHubProfile.GithubUsername;
                teacherDto.GithubProfileUrl = mainGitHubProfile.GithubProfileUrl;
                teacherDto.GithubPictureUrl = mainGitHubProfile.GithubPictureUrl;

                return teacherDto;
            }));

            return teachers.ToList();
        }, "getting teachers");
    }
}
