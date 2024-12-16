using AutoMapper;
using FluentAssertions;
using HomeAssignment.Domain.Abstractions.Contracts;
using HomeAssignment.DTOs.RequestDTOs;
using HomeAssignment.DTOs.RespondDTOs;
using HomeAssignment.DTOs.SharedDTOs;
using HomeAssignment.Persistence.Commands.GitHubProfiles;
using HomeAssignment.Persistence.Commands.Users;
using HomeAssignment.Persistence.Queries.GitHubProfiles;
using HomeAssignment.Persistence.Queries.Users;
using HomeworkAssignment.Application.Abstractions.Contracts;
using HomeworkAssignment.Application.Implementations;
using MediatR;
using NSubstitute;

namespace HomeworkAssignment.Application.Tests;

[TestFixture]
public class TeacherServiceTests
{
    private TeacherService _teacherService;
    private IMapper _mapper;
    private IMediator _mediator;
    private IPasswordHasher _passwordHasher;
    private ILogger _logger;
    private IDatabaseTransactionManager _transactionManager;

    [SetUp]
    public void SetUp()
    {
        _mapper = Substitute.For<IMapper>();
        _mediator = Substitute.For<IMediator>();
        _passwordHasher = Substitute.For<IPasswordHasher>();
        _logger = Substitute.For<ILogger>();
        _transactionManager = Substitute.For<IDatabaseTransactionManager>();

        _teacherService = new TeacherService(_logger, _mapper, _passwordHasher, _transactionManager, _mediator);
    }

    [Test]
    public async Task CreateTeacherAsync_ShouldCreateTeacherAndReturnResponse()
    {
        // Arrange
        var teacherDto = new RequestTeacherDto
        {
            FullName = "John Doe",
            Email = "john.doe@example.com",
            Password = "StrongPassword123!",
            GithubUsername = "johndoe",
            GithubProfileUrl = "https://github.com/johndoe",
            GithubPictureUrl = "https://github.com/johndoe.png"
        };

        var passwordHash = "hashedPassword123!";
        _passwordHasher.HashPassword(teacherDto.Password).Returns(passwordHash);

        var userDto = new UserDto
        {
            Id = Guid.NewGuid(),
            FullName = teacherDto.FullName,
            Email = teacherDto.Email,
            PasswordHash = "jfeoiur394ufe",
            RoleType = "teacher"
        };
        var gitHubProfileDto = new GitHubProfileDto
        {
            Id = Guid.NewGuid(),
            GithubUsername = teacherDto.GithubUsername,
            GithubProfileUrl = "http://example.com"
        };

        _mapper.Map<UserDto>(Arg.Any<object>()).Returns(userDto);
        _mapper.Map<GitHubProfileDto>(Arg.Any<object>()).Returns(gitHubProfileDto);
        _mapper.Map<RespondTeacherDto>(Arg.Any<object>()).Returns(new RespondTeacherDto { FullName = teacherDto.FullName });

        // Act
        var result = await _teacherService.CreateTeacherAsync(teacherDto);

        // Assert
        result.FullName.Should().Be(teacherDto.FullName);
        await _mediator.Received(1).Send(Arg.Is<CreateUserCommand>(cmd => cmd.UserDto == userDto), Arg.Any<CancellationToken>());
        await _mediator.Received(1).Send(Arg.Is<CreateGitHubProfileCommand>(cmd => cmd.GitHubProfileDto == gitHubProfileDto), Arg.Any<CancellationToken>());
    }
    [Test]
    public async Task UpdateTeacherAsync_ShouldUpdateTeacherAndReturnResponse()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var githubProfileId = Guid.NewGuid();

        var teacherDto = new RequestTeacherDto
        {
            FullName = "Jane Doe",
            Email = "jane.doe@example.com",
            Password = "AnotherStrongPassword!",
            GithubUsername = "janedoe",
            GithubProfileUrl = "https://github.com/janedoe",
            GithubPictureUrl = "https://github.com/janedoe.png"
        };

        var passwordHash = "hashedPassword321!";
        _passwordHasher.HashPassword(teacherDto.Password).Returns(passwordHash);

        var userDto = new UserDto
        {
            Id = userId,
            FullName = teacherDto.FullName,
            Email = teacherDto.Email,
            PasswordHash = "jfeoiur394ufe",
            RoleType = "teacher"
        };
        var gitHubProfileDto = new GitHubProfileDto
        {
            Id = githubProfileId,
            GithubUsername = teacherDto.GithubUsername,
            GithubProfileUrl = "http://example.com"
        };

        _mapper.Map<UserDto>(Arg.Any<object>()).Returns(userDto);
        _mapper.Map<GitHubProfileDto>(Arg.Any<object>()).Returns(gitHubProfileDto);
        _mapper.Map<RespondTeacherDto>(Arg.Any<object>()).Returns(new RespondTeacherDto { FullName = teacherDto.FullName });

        // Act
        var result = await _teacherService.UpdateTeacherAsync(userId, githubProfileId, teacherDto);

        // Assert
        result.FullName.Should().Be(teacherDto.FullName);
        await _mediator.Received(1).Send(Arg.Is<UpdateUserCommand>(cmd => cmd.UserDto == userDto), Arg.Any<CancellationToken>());
        await _mediator.Received(1).Send(Arg.Is<UpdateGitHubProfileCommand>(cmd => cmd.GitHubProfileDto == gitHubProfileDto), Arg.Any<CancellationToken>());
    }
    [Test]
    public async Task DeleteTeacherAsync_ShouldSendDeleteUserCommand()
    {
        // Arrange
        var userId = Guid.NewGuid();

        // Act
        await _teacherService.DeleteTeacherAsync(userId);

        // Assert
        await _mediator.Received(1).Send(Arg.Is<DeleteUserCommand>(cmd => cmd.Id == userId), Arg.Any<CancellationToken>());
    }
    [Test]
    public async Task GetTeacherByIdAsync_ShouldReturnTeacher_WhenFound()
    {
        // Arrange
        var githubProfileId = Guid.NewGuid();
        var userDto = new UserDto
        {
            Id = Guid.NewGuid(),
            FullName = "John Doe",
            Email = "john.doe@example.com",
            PasswordHash = "jfeoiur394ufe",
            RoleType = "teacher"
        };
        var gitHubProfileDto = new GitHubProfileDto
        {
            Id = githubProfileId,
            GithubUsername = "johndoe",
            GithubProfileUrl = "https://github.com/johndoe",
            GithubPictureUrl = "https://github.com/johndoe.png"
        };

        _mediator.Send(Arg.Any<GetUserByGithubProfileIdQuery>(), Arg.Any<CancellationToken>()).Returns(userDto);
        _mediator.Send(Arg.Any<GetGitHubProfileByIdQuery>(), Arg.Any<CancellationToken>()).Returns(gitHubProfileDto);

        _mapper.Map<RespondTeacherDto>(Arg.Any<object>()).Returns(new RespondTeacherDto
        {
            FullName = userDto.FullName,
            GithubUsername = gitHubProfileDto.GithubUsername
        });

        // Act
        var result = await _teacherService.GetTeacherByIdAsync(githubProfileId);

        // Assert
        result.Should().NotBeNull();
        result.GithubUsername.Should().Be("johndoe");
    }
    [Test]
    public async Task GetTeachersAsync_ShouldReturnListOfTeachers()
    {
        // Arrange
        var userDtos = new List<UserDto>
        {
            new()
            {
                Id = Guid.NewGuid(),
                FullName = "John Doe",
                Email = "john.doe@example.com",
                PasswordHash = "jfeoiur394ufe",
                RoleType = "teacher"
            },
            new()
            {
                Id = Guid.NewGuid(),
                FullName = "Jane Doe",
                Email = "jane.doe@example.com",
                PasswordHash = "jfeoiurjipj394ufe",
                RoleType = "teacher"
            }
        };

        _mediator.Send(Arg.Any<GetAllUsersByRoleQuery>(), Arg.Any<CancellationToken>()).Returns(userDtos);
        _mapper.Map<RespondTeacherDto>(Arg.Any<object>()).Returns(new RespondTeacherDto { FullName = "MappedTeacher" });

        // Act
        var result = await _teacherService.GetTeachersAsync();

        // Assert
        result.Should().HaveCount(2);
        result.All(teacher => teacher.FullName == "MappedTeacher").Should().BeTrue();
    }
}