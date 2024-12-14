using AutoMapper;
using FluentAssertions;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Database.Entities;
using HomeAssignment.DTOs.SharedDTOs;
using HomeAssignment.Persistence.Commands.GitHubProfiles;
using NSubstitute;

namespace HomeAssignment.Persistence.Tests.CommandsTests;

[TestFixture]
public class UpdateGitHubProfileCommandHandlerTests
{
    [SetUp]
    public void Setup()
    {
        _mockContext = Substitute.For<IHomeworkAssignmentDbContext>();
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<GitHubProfileDto, GitHubProfilesEntity>().ReverseMap();
        });
        _mapper = config.CreateMapper();

        _handler = new UpdateGitHubProfileCommandHandler(_mockContext, _mapper);
    }

    private IHomeworkAssignmentDbContext _mockContext;
    private IMapper _mapper;
    private UpdateGitHubProfileCommandHandler _handler;

    [Test]
    public void Should_Throw_ArgumentNullException_When_Command_Is_Null()
    {
        // Act
        Func<Task> act = async () => await _handler.Handle(null!, CancellationToken.None);

        // Assert
        act.Should().ThrowAsync<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'command')");
    }

    [Test]
    public void Should_Throw_ArgumentNullException_When_Context_Is_Null()
    {
        var act = () => new UpdateGitHubProfileCommandHandler(null!, _mapper);

        act.Should().Throw<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'context')");
    }

    [Test]
    public void Should_Throw_ArgumentNullException_When_Mapper_Is_Null()
    {
        var act = () => new UpdateGitHubProfileCommandHandler(_mockContext, null!);

        act.Should().Throw<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'mapper')");
    }

    [Test]
    public async Task Should_Update_GitHubProfile_And_Return_GitHubProfileDto()
    {
        // Arrange
        var gitHubProfileDto = new GitHubProfileDto
        {
            Id = Guid.NewGuid(),
            GithubUsername = "testuser",
            GithubProfileUrl = "https://github.com/testuser/repo"
        };
        var command = new UpdateGitHubProfileCommand(gitHubProfileDto);

        var expectedEntity = _mapper.Map<GitHubProfilesEntity>(gitHubProfileDto);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        _mockContext.GitHubProfilesEntities.Received(1).Update(Arg.Is<GitHubProfilesEntity>(entity =>
            entity.Id == expectedEntity.Id &&
            entity.GithubUsername == expectedEntity.GithubUsername &&
            entity.GithubProfileUrl == expectedEntity.GithubProfileUrl));

        result.Should().NotBeNull();
        result.Id.Should().Be(gitHubProfileDto.Id);
        result.GithubUsername.Should().Be(gitHubProfileDto.GithubUsername);
        result.GithubProfileUrl.Should().Be(gitHubProfileDto.GithubProfileUrl);
    }
}