using FluentAssertions;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Database.Entities;
using HomeAssignment.Persistence.Commands.Users;
using NSubstitute;

namespace HomeAssignment.Persistence.Tests.CommandsTests;

[TestFixture]
public class DeleteUserCommandHandlerTests
{
    [SetUp]
    public void Setup()
    {
        _mockContext = Substitute.For<IHomeworkAssignmentDbContext>();
        _handler = new DeleteUserCommandHandler(_mockContext);
    }

    private IHomeworkAssignmentDbContext _mockContext;
    private DeleteUserCommandHandler _handler;

    [Test]
    public void Should_Throw_ArgumentNullException_When_Command_Is_Null()
    {
        // Act
        var act = async () => await _handler.Handle(null!, CancellationToken.None);

        // Assert
        act.Should().ThrowAsync<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'command')");
    }

    [Test]
    public void Should_Throw_ArgumentNullException_When_Context_Is_Null()
    {
        // Act
        var act = () => new DeleteUserCommandHandler(null!);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'context')");
    }

    [Test]
    public async Task Should_Not_Throw_When_User_Not_Found()
    {
        // Arrange
        var command = new DeleteUserCommand(Guid.NewGuid());
        _mockContext.UserEntities.FindAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns((UserEntity?)null);

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().NotThrowAsync();
        await _mockContext.UserEntities.Received(1).FindAsync(command.Id, Arg.Any<CancellationToken>());
        _mockContext.UserEntities.DidNotReceive().Remove(Arg.Any<UserEntity>());
    }

    [Test]
    public async Task Should_Remove_User_When_Found()
    {
        // Arrange
        var userEntity = new UserEntity
        {
            Id = Guid.NewGuid(),
            Email = "johndoe@example.com",
            FullName = "John Doe",
            PasswordHash = "sijdiowe9u9-d3",
            RoleType = "teacher"
        };
        var command = new DeleteUserCommand(userEntity.Id);
        _mockContext.UserEntities.FindAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(userEntity);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _mockContext.UserEntities.Received(1).FindAsync(command.Id, Arg.Any<CancellationToken>());
        _mockContext.UserEntities.Received(1).Remove(userEntity);
    }
}