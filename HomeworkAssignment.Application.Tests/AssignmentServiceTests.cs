using FluentAssertions;
using HomeAssignment.Domain.Abstractions.Contracts;
using HomeAssignment.DTOs.RequestDTOs;
using HomeAssignment.DTOs.RespondDTOs;
using HomeAssignment.Persistence.Abstractions.Exceptions;
using HomeAssignment.Persistence.Commands.Assignments;
using HomeAssignment.Persistence.Queries.Assignments;
using HomeworkAssignment.Application.Abstractions.Contracts;
using HomeworkAssignment.Application.Implementations;
using MediatR;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace HomeworkAssignment.Application.Tests;

[TestFixture]
public class AssignmentServiceTests
{
    private ILogger _logger;
    private IDatabaseTransactionManager _transactionManager;
    private IMediator _mediator;
    private AssignmentService _service;

    [SetUp]
    public void Setup()
    {
        _logger = Substitute.For<ILogger>();
        _transactionManager = Substitute.For<IDatabaseTransactionManager>();
        _mediator = Substitute.For<IMediator>();

        _service = new AssignmentService(_logger, _transactionManager, _mediator);
    }

    [Test]
    public async Task CreateAssignmentAsync_ShouldReturnRespondAssignmentDto()
    {
        // Arrange
        var requestDto = new RequestAssignmentDto
        {
            Title = "Test Assignment",
            RepositoryName = "Test Repository Name",
        };
        var expectedResponse = new RespondAssignmentDto { Id = Guid.NewGuid(), Title = "Test Assignment", RepositoryName = "Test Repository Name" };

        _mediator.Send(Arg.Any<CreateAssignmentCommand>()).Returns(expectedResponse);

        // Act
        var result = await _service.CreateAssignmentAsync(requestDto);

        // Assert
        result.Should().BeEquivalentTo(expectedResponse);
        await _mediator.Received(1).Send(Arg.Any<CreateAssignmentCommand>());
    }
    [Test]
    public async Task UpdateAssignmentAsync_ShouldReturnUpdatedRespondAssignmentDto()
    {
        // Arrange
        var assignmentId = Guid.NewGuid();
        var requestDto = new RequestAssignmentDto
        {
            Title = "Test Assignment",
            RepositoryName = "Test Repository Name",
        };
        var expectedResponse = new RespondAssignmentDto { Id = assignmentId, Title = "Updated Assignment" };

        _mediator.Send(Arg.Any<UpdateAssignmentCommand>()).Returns(expectedResponse);

        // Act
        var result = await _service.UpdateAssignmentAsync(assignmentId, requestDto);

        // Assert
        result.Should().BeEquivalentTo(expectedResponse);
        await _mediator.Received(1).Send(Arg.Any<UpdateAssignmentCommand>());
    }
    [Test]
    public async Task DeleteAssignmentAsync_ShouldCallMediatorSend()
    {
        // Arrange
        var assignmentId = Guid.NewGuid();

        // Act
        await _service.DeleteAssignmentAsync(assignmentId);

        // Assert
        await _mediator.Received(1).Send(Arg.Any<DeleteAssignmentCommand>());
    }
    [Test]
    public async Task GetAssignmentByIdAsync_ShouldReturnRespondAssignmentDto()
    {
        // Arrange
        var assignmentId = Guid.NewGuid();
        var expectedResponse = new RespondAssignmentDto { Id = assignmentId, Title = "Test Assignment" };

        _mediator.Send(Arg.Any<GetAssignmentByIdQuery>()).Returns(expectedResponse);

        // Act
        var result = await _service.GetAssignmentByIdAsync(assignmentId);

        // Assert
        result.Should().BeEquivalentTo(expectedResponse);
        await _mediator.Received(1).Send(Arg.Any<GetAssignmentByIdQuery>());
    }
    [Test]
    public async Task GetAssignmentsAsync_ShouldReturnListOfRespondAssignmentDto()
    {
        // Arrange
        var expectedResponse = new List<RespondAssignmentDto>
        {
            new() { Id = Guid.NewGuid(), Title = "Assignment 1" },
            new() { Id = Guid.NewGuid(), Title = "Assignment 2" }
        };

        _mediator.Send(Arg.Any<GetAllAssignmentsQuery>()).Returns(expectedResponse);

        // Act
        var result = await _service.GetAssignmentsAsync();

        // Assert
        result.Should().BeEquivalentTo(expectedResponse);
        await _mediator.Received(1).Send(Arg.Any<GetAllAssignmentsQuery>());
    }
    [Test]
    public void CreateAssignmentAsync_ShouldThrowServiceOperationException_WhenExceptionOccurs()
    {
        // Arrange
        var requestDto = new RequestAssignmentDto
        {
            Title = "Test Assignment",
            RepositoryName = "Test Repository Name"
        };
        _mediator.Send(Arg.Any<CreateAssignmentCommand>()).Throws(new Exception("Database error"));

        // Act
        Func<Task> act = async () => await _service.CreateAssignmentAsync(requestDto);

        // Assert
        act.Should().ThrowAsync<ServiceOperationException>().WithMessage("Error during creating assignment.");
        _logger.Received().Log(Arg.Is<string>(s => s.Contains("Error during creating assignment")));
    }
}