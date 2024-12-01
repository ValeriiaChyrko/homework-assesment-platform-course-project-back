using FluentAssertions;
using HomeAssignment.DTOs.RequestDTOs;
using HomeAssignment.DTOs.RespondDTOs;
using HomeworkAssignment.Application.Abstractions;
using HomeworkAssignment.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;

namespace HomeworkAssignment.API.Tests;

[TestFixture]
public class AssignmentControllerTests
{
    private IAssignmentService _assignmentService;
    private AssignmentController _controller;

    [SetUp]
    public void SetUp()
    {
        _assignmentService = Substitute.For<IAssignmentService>();
        _controller = new AssignmentController(_assignmentService);
    }

    [Test]
    public async Task Get_ReturnsOkResult_WithAssignments()
    {
        // Arrange
        var mockAssignments = new List<RespondAssignmentDto>
        {
            new() 
        };
        _assignmentService.GetAssignmentsAsync().Returns(Task.FromResult<IReadOnlyList<RespondAssignmentDto>>(mockAssignments));

        // Act
        var result = await _controller.Get();

        // Assert
        result.Result.Should().BeOfType<ObjectResult>().Which.StatusCode.Should().Be(200);
        result.Result.As<ObjectResult>().Value.Should().BeEquivalentTo(mockAssignments);
    }
    [Test]
    public async Task Get_WithValidId_ReturnsOkResult_WithAssignment()
    {
        // Arrange
        var id = Guid.NewGuid();
        var expectedResponse = new RespondAssignmentDto
        {
            Id = id,
            Title = "Sample Title",
            RepositoryName = "Sample Repository"
        };
        _assignmentService.GetAssignmentByIdAsync(id)!.Returns(Task.FromResult(expectedResponse));

        // Act
        var result = await _controller.Get(id);

        // Assert
        result.Should().BeOfType<ActionResult<RespondAssignmentDto>>()
            .Which.Result.Should().BeOfType<ObjectResult>()
            .Which.StatusCode.Should().Be(StatusCodes.Status200OK);
            
        result.Result.As<ObjectResult>().Value.Should().BeEquivalentTo(expectedResponse);
    }
    [Test]
    public async Task Get_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        _assignmentService.GetAssignmentByIdAsync(id)!.Returns(Task.FromResult<RespondAssignmentDto>(null!));

        // Act
        var result = await _controller.Get(id);

        // Assert
        result.Should().BeOfType<ActionResult<RespondAssignmentDto>>()
            .Which.Result.Should().BeOfType<StatusCodeResult>()
            .Which.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }
    [Test]
    public async Task Delete_ReturnsOkResult_WithId()
    {
        // Arrange
        var id = Guid.NewGuid();
        _assignmentService.DeleteAssignmentAsync(id).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Delete(id);

        // Assert
        result.Result.Should().BeOfType<ObjectResult>().Which.StatusCode.Should().Be(200);
        result.Result.As<ObjectResult>().Value.Should().BeEquivalentTo(id);
    }
    [Test]
    public async Task Create_ReturnsCreatedResult_WithId()
    {
        // Arrange
        var request = new RequestAssignmentDto
        {
            Title = "string",
            RepositoryName = "string",
        };
        var expected = new RespondAssignmentDto
        {
            Id = Guid.NewGuid(),
            Title = "string",
            RepositoryName = "string",
        };
            
        _assignmentService.CreateAssignmentAsync(Arg.Any<RequestAssignmentDto>())
            .Returns(Task.FromResult(expected));

        // Act
        var result = await _controller.Create(request);

        // Assert
        result.Result.Should().BeOfType<ObjectResult>().Which.StatusCode.Should().Be(201);
        result.Result.As<ObjectResult>().Value.Should().BeEquivalentTo(expected);
    }
    [Test]
    public async Task Update_ReturnsOkResult_WithUpdatedAssignment()
    {
        // Arrange
        var id = Guid.NewGuid();
        var request = new RequestAssignmentDto
        {
            Title = "Updated Title",
            RepositoryName = "Updated Repository"
        };
        var expectedResponse = new RespondAssignmentDto
        {
            Id = id,
            Title = "Updated Title",
            RepositoryName = "Updated Repository"
        };
            
        _assignmentService.UpdateAssignmentAsync(id, request).Returns(Task.FromResult(expectedResponse));

        // Act
        var result = await _controller.Update(id, request);

        // Assert
        result.Should().BeOfType<ActionResult<RespondAssignmentDto>>()
            .Which.Result.Should().BeOfType<ObjectResult>()
            .Which.StatusCode.Should().Be(StatusCodes.Status200OK);
            
        result.Result.As<ObjectResult>().Value.Should().BeEquivalentTo(expectedResponse);
    }
}