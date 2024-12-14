using FluentAssertions;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Database.Entities;
using HomeAssignment.Persistence.Commands.Attempts;
using NSubstitute;

namespace HomeAssignment.Persistence.Tests.CommandsTests;

[TestFixture]
public class DeleteAttemptCommandHandlerTests
{
    [SetUp]
    public void Setup()
    {
        _mockContext = Substitute.For<IHomeworkAssignmentDbContext>();
        _handler = new DeleteAttemptCommandHandler(_mockContext);
    }

    private IHomeworkAssignmentDbContext _mockContext;
    private DeleteAttemptCommandHandler _handler;

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
        var act = () => new DeleteAttemptCommandHandler(null!);

        act.Should().Throw<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'context')");
    }

    [Test]
    public async Task Should_Not_Remove_Any_Entity_When_Attempt_Is_Not_Found()
    {
        // Arrange
        var command = new DeleteAttemptCommand(Guid.NewGuid());

        _mockContext.AttemptEntities.FindAsync(command.Id, CancellationToken.None)
            .Returns((AttemptEntity?)null);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _mockContext.AttemptEntities.DidNotReceive().Remove(Arg.Any<AttemptEntity>());
    }

    [Test]
    public async Task Should_Remove_Attempt_When_Attempt_Is_Found()
    {
        // Arrange
        var command = new DeleteAttemptCommand(Guid.NewGuid());
        var attemptEntity = new AttemptEntity { Id = command.Id };

        _mockContext.AttemptEntities.FindAsync(command.Id, CancellationToken.None)
            .Returns(attemptEntity); // Simulate attempt found

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _mockContext.AttemptEntities.Received(1).Remove(attemptEntity);
    }
}