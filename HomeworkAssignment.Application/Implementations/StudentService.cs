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

public class StudentService : BaseService<StudentService>, IStudentService
{
    private readonly ILogger<StudentService> _logger;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly IPasswordHasher _passwordHasher;

    public StudentService(IDatabaseTransactionManager transactionManager, IMediator mediator,
        IPasswordHasher passwordHasher, ILogger<StudentService> logger, IMapper mapper)
        : base(logger, transactionManager)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mapper = mapper;
    }

    public async Task<RespondStudentDto> CreateStudentAsync(RequestStudentDto studentDto,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Started creating student: {@StudentDto}", studentDto);
        var result = await ExecuteTransactionAsync(async () =>
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
            await _mediator.Send(new CreateGitHubProfileCommand(_mapper.Map<GitHubProfileDto>(student)),
                cancellationToken);

            return _mapper.Map<RespondStudentDto>(student);
        }, cancellationToken: cancellationToken);

        _logger.LogInformation("Successfully created student with ID: {StudentId}", result.UserId);
        return result;
    }

    public async Task<RespondStudentDto> UpdateStudentAsync(Guid userId, Guid githubProfileId,
        RequestStudentDto studentDto, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(
            "Started updating student with User ID: {User Id} and GitHub Profile ID: {GithubProfileId}", userId,
            githubProfileId);
        var result = await ExecuteTransactionAsync(async () =>
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
            await _mediator.Send(new UpdateGitHubProfileCommand(_mapper.Map<GitHubProfileDto>(student)),
                cancellationToken);

            return _mapper.Map<RespondStudentDto>(student);
        }, cancellationToken: cancellationToken);

        _logger.LogInformation("Successfully updated student with User ID: {User Id}", userId);
        return result;
    }

    public async Task DeleteStudentAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Started deleting student with User ID: {User Id}", userId);
        await ExecuteTransactionAsync(
            async () => { await _mediator.Send(new DeleteUserCommand(userId), cancellationToken); },
            cancellationToken: cancellationToken);
        _logger.LogInformation("Successfully deleted student with User ID: {User Id}", userId);
    }

    public async Task<RespondStudentDto?> GetStudentByIdAsync(Guid githubProfileId,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Started retrieving student by GitHub Profile ID: {GithubProfileId}", githubProfileId);
        var result = await ExecuteWithExceptionHandlingAsync(async () =>
        {
            var userDto = await _mediator.Send(new GetUserByGithubProfileIdQuery(githubProfileId), cancellationToken);
            if (userDto == null) return null;

            var gitHubProfileDto =
                await _mediator.Send(new GetGitHubProfileByIdQuery(githubProfileId), cancellationToken);
            var studentDto = _mapper.Map<RespondStudentDto>(userDto);

            if (gitHubProfileDto != null)
            {
                studentDto.GitHubProfileId = gitHubProfileDto.Id;
                studentDto.GithubUsername = gitHubProfileDto.GithubUsername;
                studentDto.GithubProfileUrl = gitHubProfileDto.GithubProfileUrl;
                studentDto.GithubPictureUrl = gitHubProfileDto.GithubPictureUrl;
            }

            return studentDto;
        });

        if (result != null)
            _logger.LogInformation("Successfully retrieved student with GitHub Profile ID: {GithubProfileId}",
                githubProfileId);
        else
            _logger.LogWarning("No student found with GitHub Profile ID: {GithubProfileId}", githubProfileId);

        return result;
    }

    public async Task<PagedList<RespondStudentDto>> GetStudentsAsync(RequestUserFilterParameters filterParameters, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Started retrieving all students");
        var query = new GetAllUsersByRoleQuery(filterParameters, UserRoles.Student);
        var result = await ExecuteWithExceptionHandlingAsync(async () =>
        {
            var userDtos = await _mediator.Send(query, cancellationToken);
            var students = await Task.WhenAll(userDtos.Items.Select(async user =>
            {
                var studentDto = _mapper.Map<RespondStudentDto>(user);
                var gitHubProfiles =
                    await _mediator.Send(new GetAllGitHubProfilesByUserIdQuery(user.Id), cancellationToken);
                var mainGitHubProfile = gitHubProfiles?.FirstOrDefault();

                if (mainGitHubProfile == null) return studentDto;
                
                studentDto.GitHubProfileId = mainGitHubProfile.Id;
                studentDto.GithubUsername = mainGitHubProfile.GithubUsername;
                studentDto.GithubProfileUrl = mainGitHubProfile.GithubProfileUrl;
                studentDto.GithubPictureUrl = mainGitHubProfile.GithubPictureUrl;

                return studentDto;
            }));

            return new PagedList<RespondStudentDto>(students.ToList(), userDtos.TotalCount, userDtos.Page, userDtos.PageSize);
        });

        _logger.LogInformation("Successfully retrieved all students");
        return result;
    }
}