using FluentAssertions;
using HomeAssignment.Domain.Abstractions.Contracts;
using HomeAssignment.DTOs.RequestDTOs;
using HomeAssignment.DTOs.RespondDTOs;
using HomeAssignment.Persistence.Commands.Attempts;
using HomeAssignment.Persistence.Queries.Attempts;
using HomeworkAssignment.Application.Abstractions.Contracts;
using HomeworkAssignment.Application.Implementations;
using MediatR;
using NSubstitute;

namespace HomeworkAssignment.Application.Tests;

[TestFixture]
public class AttemptServiceTests
{
    private ILogger _logger;
    private IDatabaseTransactionManager _transactionManager;
    private IMediator _mediator;
    private AttemptService _service;

    [SetUp]
    public void SetUp()
    {
        _logger = Substitute.For<ILogger>();
        _transactionManager = Substitute.For<IDatabaseTransactionManager>();
        _mediator = Substitute.For<IMediator>();
        _service = new AttemptService(_logger, _transactionManager, _mediator);
    }

    [Test]
    public async Task CreateAttemptAsync_ShouldReturnCreatedAttempt()
    {
        // Arrange
        var request = new RequestAttemptDto { AssignmentId = Guid.NewGuid(), StudentId = Guid.NewGuid() };
        var response = new RespondAttemptDto
            { Id = Guid.NewGuid(), AssignmentId = request.AssignmentId, StudentId = request.StudentId };

        _mediator.Send(Arg.Any<CreateAttemptCommand>()).Returns(response);

        // Act
        var result = await _service.CreateAttemptAsync(request);

        // Assert
        result.Should().BeEquivalentTo(response);
        await _mediator.Received(1).Send(Arg.Is<CreateAttemptCommand>(cmd => cmd.AttemptDto == request));
    }
    [Test]
    public async Task UpdateAttemptAsync_ShouldReturnUpdatedAttempt()
    {
        // Arrange
        var id = Guid.NewGuid();
        var request = new RequestAttemptDto { AssignmentId = Guid.NewGuid(), StudentId = Guid.NewGuid() };
        var response = new RespondAttemptDto
            { Id = id, AssignmentId = request.AssignmentId, StudentId = request.StudentId };

        _mediator.Send(Arg.Any<UpdateAttemptCommand>()).Returns(response);

        // Act
        var result = await _service.UpdateAttemptAsync(id, request);

        // Assert
        result.Should().BeEquivalentTo(response);
        await _mediator.Received(1)
            .Send(Arg.Is<UpdateAttemptCommand>(cmd => cmd.Id == id && cmd.AttemptDto == request));
    }
    [Test]
    public async Task DeleteAttemptAsync_ShouldCallMediatorWithCorrectCommand()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        await _service.DeleteAttemptAsync(id);

        // Assert
        await _mediator.Received(1).Send(Arg.Is<DeleteAttemptCommand>(cmd => cmd.Id == id));
    }
    [Test]
    public async Task GetAttemptByIdAsync_ShouldReturnCorrectAttempt()
    {
        // Arrange
        var id = Guid.NewGuid();
        var response = new RespondAttemptDto { Id = id };

        _mediator.Send(Arg.Any<GetAttemptByIdQuery>()).Returns(response);

        // Act
        var result = await _service.GetAttemptByIdAsync(id);

        // Assert
        result.Should().BeEquivalentTo(response);
        await _mediator.Received(1).Send(Arg.Is<GetAttemptByIdQuery>(query => query.Id == id));
    }
    [Test]
    public async Task GetAttemptsByAssignmentIdAsync_ShouldReturnListOfAttempts()
    {
        // Arrange
        var assignmentId = Guid.NewGuid();
        var responses = new List<RespondAttemptDto> { new RespondAttemptDto { AssignmentId = assignmentId } };

        _mediator.Send(Arg.Any<GetAllAttemptsByAssignmentIdQuery>()).Returns(responses);

        // Act
        var result = await _service.GetAttemptsByAssignmentIdAsync(assignmentId);

        // Assert
        result.Should().BeEquivalentTo(responses);
        await _mediator.Received(1)
            .Send(Arg.Is<GetAllAttemptsByAssignmentIdQuery>(query => query.AssignmentId == assignmentId));
    }
    [Test]
    public async Task GetLastAttemptByAssignmentIdAsync_ShouldReturnLastAttempt()
    {
        // Arrange
        var assignmentId = Guid.NewGuid();
        var response = new RespondAttemptDto { AssignmentId = assignmentId };

        _mediator.Send(Arg.Any<GetLastAttemptByAssignmentIdQuery>()).Returns(response);

        // Act
        var result = await _service.GetLastAttemptByAssignmentIdAsync(assignmentId);

        // Assert
        result.Should().BeEquivalentTo(response);
        await _mediator.Received(1)
            .Send(Arg.Is<GetLastAttemptByAssignmentIdQuery>(query => query.AssignmentId == assignmentId));
    }
    [Test]
    public async Task GetStudentAttemptsAsync_ShouldReturnListOfStudentAttempts()
    {
        // Arrange
        var assignmentId = Guid.NewGuid();
        var studentId = Guid.NewGuid();
        var responses = new List<RespondAttemptDto>
            { new RespondAttemptDto { AssignmentId = assignmentId, StudentId = studentId } };

        _mediator.Send(Arg.Any<GetAllAttemptsByStudentIdQuery>()).Returns(responses);

        // Act
        var result = await _service.GetStudentAttemptsAsync(assignmentId, studentId);

        // Assert
        result.Should().BeEquivalentTo(responses);
        await _mediator.Received(1).Send(Arg.Is<GetAllAttemptsByStudentIdQuery>(query =>
            query.AssignmentId == assignmentId && query.StudentId == studentId));
    }
}