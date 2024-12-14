using FluentAssertions;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Database.Entities;
using HomeAssignment.Persistence.Commands.GitHubProfiles;
using NSubstitute;

namespace HomeAssignment.Persistence.Tests.CommandsTests;

[TestFixture]
public class DeleteGitHubProfileCommandHandlerTests
{
    [SetUp]
    public void Setup()
    {
        _mockContext = Substitute.For<IHomeworkAssignmentDbContext>();
        _handler = new DeleteGitHubProfileCommandHandler(_mockContext);
    }

    private IHomeworkAssignmentDbContext _mockContext;
    private DeleteGitHubProfileCommandHandler _handler;

    [Test]
    public void Should_Throw_ArgumentNullException_When_Command_Is_Null()
    {
        var act = async () => await _handler.Handle(null!, CancellationToken.None);

        act.Should().ThrowAsync<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'command')");
    }

    [Test]
    public void Should_Throw_ArgumentNullException_When_Context_Is_Null()
    {
        var act = () => new DeleteGitHubProfileCommandHandler(null!);

        act.Should().Throw<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'context')");
    }

    [Test]
    public async Task Should_Delete_GitHubProfile_If_Entity_Exists()
    {
        // Arrange
        var command = new DeleteGitHubProfileCommand(Guid.NewGuid());
        var gitHubProfileEntity = new GitHubProfilesEntity
        {
            Id = command.Id,
            GithubUsername = "string",
            GithubProfileUrl = "string"
        };

        _mockContext.GitHubProfilesEntities.FindAsync(command.Id, CancellationToken.None)
            .Returns(gitHubProfileEntity);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _mockContext.GitHubProfilesEntities.Received(1).Remove(gitHubProfileEntity);
    }

    [Test]
    public async Task Should_Not_Throw_When_Entity_Does_Not_Exist()
    {
        // Arrange
        var gitHubProfileId = Guid.NewGuid();
        var command = new DeleteGitHubProfileCommand(gitHubProfileId);

        _mockContext.GitHubProfilesEntities.FindAsync(gitHubProfileId, Arg.Any<CancellationToken>())
            .Returns((GitHubProfilesEntity?)null);

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().NotThrowAsync();
        _mockContext.GitHubProfilesEntities.DidNotReceive().Remove(Arg.Any<GitHubProfilesEntity>());
    }
}