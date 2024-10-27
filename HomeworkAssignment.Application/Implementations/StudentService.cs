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

public class StudentService : IStudentService
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IDatabaseTransactionManager _transactionManager;
    private readonly IMediator _mediator;

    public StudentService(ILogger logger, IMapper mapper, IPasswordHasher passwordHasher,
        IDatabaseTransactionManager transactionManager, IMediator mediator)
    {
        _logger = logger;
        _mapper = mapper;
        _passwordHasher = passwordHasher;
        _transactionManager = transactionManager;
        _mediator = mediator;
    }

    public async Task<RespondStudentDto> CreateStudentAsync(RequestStudentDto studentDto,
        CancellationToken cancellationToken = default)
    {
        await using var transaction = await _transactionManager.BeginTransactionAsync();
        try
        {
            var passwordHash = _passwordHasher.HashPassword(studentDto.Password);

            var student = Student.Create(
                studentDto.FullName,
                studentDto.Email,
                passwordHash,
                studentDto.GithubUsername,
                studentDto.GithubAccessToken,
                studentDto.GithubProfileUrl,
                studentDto.GithubPictureUrl
            );

            var userDto = _mapper.Map<UserDto>(student);
            await _mediator.Send(new CreateUserCommand(userDto), cancellationToken);

            var profileDto = _mapper.Map<GitHubProfileDto>(student);
            await _mediator.Send(new CreateGitHubProfileCommand(profileDto), cancellationToken);

            await _transactionManager.CommitAsync(transaction, cancellationToken);
            return _mapper.Map<RespondStudentDto>(student);
        }
        catch (Exception ex)
        {
            await _transactionManager.RollbackAsync(transaction, cancellationToken);
            _logger.Log($"Error creating student {ex.InnerException}. Using rollback transaction.");

            throw new Exception("Error creating student", ex);
        }
    }

    public async Task<RespondStudentDto> UpdateStudentAsync(Guid userId, Guid githubProfileId,
        RequestStudentDto studentDto, CancellationToken cancellationToken = default)
    {
        await using var transaction = await _transactionManager.BeginTransactionAsync();
        try
        {
            var passwordHash = _passwordHasher.HashPassword(studentDto.Password);

            var student = Student.Create(
                studentDto.FullName,
                studentDto.Email,
                passwordHash,
                studentDto.GithubUsername,
                studentDto.GithubAccessToken,
                studentDto.GithubProfileUrl,
                studentDto.GithubPictureUrl
            );
            student.UserId = userId;
            student.GitHubProfileId = githubProfileId;

            var userDto = _mapper.Map<UserDto>(student);
            await _mediator.Send(new UpdateUserCommand(userDto), cancellationToken);

            var profileDto = _mapper.Map<GitHubProfileDto>(student);
            await _mediator.Send(new UpdateGitHubProfileCommand(profileDto), cancellationToken);

            await _transactionManager.CommitAsync(transaction, cancellationToken);
            return _mapper.Map<RespondStudentDto>(student);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            _logger.Log($"Error updating student {ex.InnerException}. Using rollback transaction.");

            throw new Exception("Error updating student", ex);
        }
    }

    public async Task DeleteStudentAsync(Guid userId, CancellationToken cancellationToken = default)
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
            _logger.Log($"Error deleting student {ex.InnerException}. Using rollback transaction.");

            throw new Exception("Error getting student", ex);
        }
    }

    public async Task<RespondStudentDto?> GetStudentByIdAsync(Guid userId, Guid githubProfileId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var userDto = await _mediator.Send(new GetUserByIdQuery(userId), cancellationToken);
            if (userDto == null) return null;

            var gitHubProfileDto =
                await _mediator.Send(new GetGitHubProfileByIdQuery(githubProfileId), cancellationToken);

            var studentWithProfileDto = _mapper.Map<RespondStudentDto>(userDto);
            if (gitHubProfileDto == null) return studentWithProfileDto;

            studentWithProfileDto.GithubUsername = gitHubProfileDto.GithubUsername;
            studentWithProfileDto.GithubAccessToken = gitHubProfileDto.GithubAccessToken;
            studentWithProfileDto.GithubProfileUrl = gitHubProfileDto.GithubProfileUrl;
            studentWithProfileDto.GithubPictureUrl = gitHubProfileDto.GithubPictureUrl;

            return studentWithProfileDto;
        }
        catch (Exception ex)
        {
            _logger.Log($"Error getting student {ex.InnerException}.");

            throw new Exception("Error getting student", ex);
        }
    }

    public async Task<IReadOnlyList<RespondStudentDto>> GetStudentsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var userDtos = await _mediator.Send(new GetAllUsersByRoleQuery(UserRoles.Student), cancellationToken);

            var studentDtos = await Task.WhenAll(userDtos.Select(async user =>
            {
                var studentWithProfileDto = _mapper.Map<RespondStudentDto>(user);

                var gitHubProfiles =
                    await _mediator.Send(new GetAllGitHubProfilesByUserIdQuery(user.Id), cancellationToken);
                var mainGitHubProfile = gitHubProfiles?.FirstOrDefault();

                if (mainGitHubProfile != null)
                {
                    studentWithProfileDto.GithubUsername = mainGitHubProfile.GithubUsername;
                    studentWithProfileDto.GithubAccessToken = mainGitHubProfile.GithubAccessToken;
                    studentWithProfileDto.GithubProfileUrl = mainGitHubProfile.GithubProfileUrl;
                    studentWithProfileDto.GithubPictureUrl = mainGitHubProfile.GithubPictureUrl;
                }

                return studentWithProfileDto;
            }));

            return studentDtos.ToList();
        }
        catch (Exception ex)
        {
            _logger.Log($"Error getting students entities {ex.InnerException}.");
            throw new Exception("Error getting students entities", ex);
        }
    }
}