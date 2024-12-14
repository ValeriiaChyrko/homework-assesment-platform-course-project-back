using FluentAssertions;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Database.Entities;
using HomeAssignment.Persistence.Commands.Assignments;
using NSubstitute;

namespace HomeAssignment.Persistence.Tests.CommandsTests;

[TestFixture]
public class DeleteAssignmentCommandHandlerTests
{
    [SetUp]
    public void Setup()
    {
        _mockContext = Substitute.For<IHomeworkAssignmentDbContext>();
        _handler = new DeleteAssignmentCommandHandler(_mockContext);
    }

    private IHomeworkAssignmentDbContext _mockContext;
    private DeleteAssignmentCommandHandler _handler;

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
        var act = () => new DeleteAssignmentCommandHandler(null!);

        act.Should().Throw<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'context')");
    }

    [Test]
    public async Task Should_Not_Throw_When_Assignment_Not_Found()
    {
        // Arrange
        var command = new DeleteAssignmentCommand(Guid.NewGuid());
        _mockContext.AssignmentEntities.FindAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns((AssignmentEntity?)null);

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().NotThrowAsync();
        await _mockContext.AssignmentEntities.Received(1).FindAsync(command.Id, Arg.Any<CancellationToken>());
        _mockContext.AssignmentEntities.DidNotReceive().Remove(Arg.Any<AssignmentEntity>());
    }

    [Test]
    public async Task Should_Remove_Assignment_When_Found()
    {
        // Arrange
        var assignmentEntity = new AssignmentEntity
        {
            Id = Guid.NewGuid(),
            Title = "string",
            RepositoryName = "string"
        };
        var command = new DeleteAssignmentCommand(assignmentEntity.Id);
        _mockContext.AssignmentEntities.FindAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(assignmentEntity);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _mockContext.AssignmentEntities.Received(1).FindAsync(command.Id, Arg.Any<CancellationToken>());
        _mockContext.AssignmentEntities.Received(1).Remove(assignmentEntity);
    }
}