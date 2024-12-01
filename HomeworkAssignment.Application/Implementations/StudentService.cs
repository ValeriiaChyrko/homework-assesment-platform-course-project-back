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

public class StudentService : BaseService, IStudentService
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly IPasswordHasher _passwordHasher;

    public StudentService(ILogger logger, IMapper mapper, IPasswordHasher passwordHasher, IDatabaseTransactionManager transactionManager, IMediator mediator)
        : base(logger, transactionManager)
    {
        _mapper = mapper;
        _passwordHasher = passwordHasher;
        _mediator = mediator;
    }

    public async Task<RespondStudentDto> CreateStudentAsync(RequestStudentDto studentDto, CancellationToken cancellationToken = default)
    {
        return await ExecuteWithTransactionAsync(async () =>
        {
            var passwordHash = _passwordHasher.HashPassword(studentDto.Password);
            var student = Student.Create(
                studentDto.FullName,
                studentDto.Email,
                passwordHash,
                studentDto.GithubUsername,
                studentDto.GithubProfileUrl,
                studentDto.GithubPictureUrl
            );

            await _mediator.Send(new CreateUserCommand(_mapper.Map<UserDto>(student)), cancellationToken);
            await _mediator.Send(new CreateGitHubProfileCommand(_mapper.Map<GitHubProfileDto>(student)), cancellationToken);

            return _mapper.Map<RespondStudentDto>(student);
        }, "creating student", cancellationToken);
    }

    public async Task<RespondStudentDto> UpdateStudentAsync(Guid userId, Guid githubProfileId, RequestStudentDto studentDto, CancellationToken cancellationToken = default)
    {
        return await ExecuteWithTransactionAsync(async () =>
        {
            var passwordHash = _passwordHasher.HashPassword(studentDto.Password);

            var student = Student.Create(
                studentDto.FullName,
                studentDto.Email,
                passwordHash,
                studentDto.GithubUsername,
                studentDto.GithubProfileUrl,
                studentDto.GithubPictureUrl
            );

            student.UserId = userId;
            student.GitHubProfileId = githubProfileId;

            await _mediator.Send(new UpdateUserCommand(_mapper.Map<UserDto>(student)), cancellationToken);
            await _mediator.Send(new UpdateGitHubProfileCommand(_mapper.Map<GitHubProfileDto>(student)), cancellationToken);

            return _mapper.Map<RespondStudentDto>(student);
        }, "updating student", cancellationToken);
    }

    public async Task DeleteStudentAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        await ExecuteWithTransactionAsync(async () =>
        {
            await _mediator.Send(new DeleteUserCommand(userId), cancellationToken);
            return Task.CompletedTask;
        }, "deleting student", cancellationToken);
    }

    public async Task<RespondStudentDto?> GetStudentByIdAsync(Guid githubProfileId, CancellationToken cancellationToken = default)
    {
        return await ExecuteWithExceptionHandlingAsync(async () =>
        {
            var userDto = await _mediator.Send(new GetUserByGithubProfileIdQuery(githubProfileId), cancellationToken);
            if (userDto == null) return null;

            var gitHubProfileDto = await _mediator.Send(new GetGitHubProfileByIdQuery(githubProfileId), cancellationToken);

            var studentDto = _mapper.Map<RespondStudentDto>(userDto);
            if (gitHubProfileDto == null) return studentDto;
            
            studentDto.GitHubProfileId = gitHubProfileDto.Id;
            studentDto.GithubUsername = gitHubProfileDto.GithubUsername;
            studentDto.GithubProfileUrl = gitHubProfileDto.GithubProfileUrl;
            studentDto.GithubPictureUrl = gitHubProfileDto.GithubPictureUrl;

            return studentDto;
        }, "getting student by ID");
    }

    public async Task<IReadOnlyList<RespondStudentDto>> GetStudentsAsync(CancellationToken cancellationToken = default)
    {
        return await ExecuteWithExceptionHandlingAsync(async () =>
        {
            var userDtos = await _mediator.Send(new GetAllUsersByRoleQuery(UserRoles.Student), cancellationToken);

            var students = await Task.WhenAll(userDtos.Select(async user =>
            {
                var studentDto = _mapper.Map<RespondStudentDto>(user);

                var gitHubProfiles = await _mediator.Send(new GetAllGitHubProfilesByUserIdQuery(user.Id), cancellationToken);
                var mainGitHubProfile = gitHubProfiles?.FirstOrDefault();

                if (mainGitHubProfile == null) return studentDto;
                
                studentDto.GitHubProfileId = mainGitHubProfile.Id;
                studentDto.GithubUsername = mainGitHubProfile.GithubUsername;
                studentDto.GithubProfileUrl = mainGitHubProfile.GithubProfileUrl;
                studentDto.GithubPictureUrl = mainGitHubProfile.GithubPictureUrl;

                return studentDto;
            }));

            return students.ToList();
        }, "getting students");
    }
}