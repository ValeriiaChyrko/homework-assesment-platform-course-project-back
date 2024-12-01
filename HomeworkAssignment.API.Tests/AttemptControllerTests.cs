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
public class AttemptControllerTests
{
    private IAttemptService _attemptService;
    private AttemptController _controller;

    [SetUp]
    public void SetUp()
    {
        _attemptService = Substitute.For<IAttemptService>();
        _controller = new AttemptController(_attemptService);
    }

    [Test]
    public async Task ByAssignmentId_ReturnsOkResult_WithAttempts()
    {
        // Arrange
        var assignmentId = Guid.NewGuid();
        var mockAttempts = new List<RespondAttemptDto>
        {
            new()
            {
                Id = Guid.NewGuid(),
                StudentId = Guid.NewGuid(),
                AssignmentId = assignmentId,
                BranchName = "Math",
                FinishedAt = DateTime.UtcNow,
                AttemptNumber = 1,
                CompilationScore = 85,
                TestsScore = 90,
                QualityScore = 80,
                FinalScore = 85
            }
        };
        _attemptService.GetAttemptsByAssignmentIdAsync(assignmentId)
            .Returns(Task.FromResult<IReadOnlyList<RespondAttemptDto>>(mockAttempts));

        // Act
        var result = await _controller.ByAssignmentId(assignmentId);

        // Assert
        result.Result.Should().BeOfType<ObjectResult>()
            .Which.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Result.As<ObjectResult>().Value.Should().BeEquivalentTo(mockAttempts);
    }
    [Test]
    public async Task LastAttemptByAssignmentId_ReturnsOkResult_WithLastAttempt()
    {
        // Arrange
        var assignmentId = Guid.NewGuid();
        var mockLastAttempt = new RespondAttemptDto
        {
            Id = Guid.NewGuid(),
            StudentId = Guid.NewGuid(),
            AssignmentId = assignmentId,
            BranchName = "Science",
            FinishedAt = DateTime.UtcNow,
            AttemptNumber = 2,
            CompilationScore = 90,
            TestsScore = 95,
            QualityScore = 88,
            FinalScore = 91
        };
        _attemptService.GetLastAttemptByAssignmentIdAsync(assignmentId)
            .Returns(Task.FromResult(mockLastAttempt));

        // Act
        var result = await _controller.LastAttemptByAssignmentId(assignmentId);

        // Assert
        result.Should().BeOfType<ActionResult<RespondAttemptDto>>()
            .Which.Result.Should().BeOfType<ObjectResult>()
            .Which.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Result.As<ObjectResult>().Value.Should().BeEquivalentTo(mockLastAttempt);
    }
    [Test]
    public async Task ByStudentId_ReturnsOkResult_WithAttempts()
    {
        // Arrange
        var studentId = Guid.NewGuid();
        var mockAttempts = new List<RespondAttemptDto>
        {
            new()
            {
                Id = Guid.NewGuid(),
                StudentId = studentId,
                AssignmentId = Guid.NewGuid(),
                BranchName = "History",
                FinishedAt = DateTime.UtcNow,
                AttemptNumber = 1,
                CompilationScore = 75,
                TestsScore = 80,
                QualityScore = 70,
                FinalScore = 75
            }
        };
        _attemptService.GetAttemptsByStudentIdAsync(studentId)
            .Returns(Task.FromResult<IReadOnlyList<RespondAttemptDto>>(mockAttempts));

        // Act
        var result = await _controller.ByStudentId(studentId);

        // Assert
        result.Result.Should().BeOfType<ObjectResult>()
            .Which.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Result.As<ObjectResult>().Value.Should().BeEquivalentTo(mockAttempts);
    }
    [Test]
    public async Task Get_WithValidId_ReturnsOkResult_WithAttempt()
    {
        // Arrange
        var id = Guid.NewGuid();
        var expectedResponse = new RespondAttemptDto
        {
            Id = id,
            StudentId = Guid.NewGuid(),
            AssignmentId = Guid.NewGuid(),
            BranchName = "Geography",
            FinishedAt = DateTime.UtcNow,
            AttemptNumber = 1,
            CompilationScore = 80,
            TestsScore = 85,
            QualityScore = 90,
            FinalScore = 85
        };
        _attemptService.GetAttemptByIdAsync(id)!.Returns(Task .FromResult(expectedResponse));

        // Act
        var result = await _controller.Get(id);

        // Assert
        result.Should().BeOfType<ActionResult<RespondAttemptDto>>()
            .Which.Result.Should().BeOfType<ObjectResult>()
            .Which.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Result.As<ObjectResult>().Value.Should().BeEquivalentTo(expectedResponse);
    }
    [Test]
    public async Task Get_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        _attemptService.GetAttemptByIdAsync(id)!.Returns(Task.FromResult<RespondAttemptDto>(null!));

        // Act
        var result = await _controller.Get(id);

        // Assert
        result.Should().BeOfType<ActionResult<RespondAttemptDto>>()
            .Which.Result.Should().BeOfType<StatusCodeResult>()
            .Which.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }
    [Test]
    public async Task Create_ReturnsCreatedResult_WithCreatedAttempt()
    {
        // Arrange
        var request = new RequestAttemptDto
        {
            StudentId = Guid.NewGuid(),
            AssignmentId = Guid.NewGuid(),
            BranchName = "Literature",
            AttemptNumber = 1,
            CompilationScore = 100,
            TestsScore = 95,
            QualityScore = 90
        };
        var expectedResponse = new RespondAttemptDto
        {
            Id = Guid.NewGuid(),
            StudentId = request.StudentId,
            AssignmentId = request.AssignmentId,
            BranchName = request.BranchName,
            AttemptNumber = request.AttemptNumber,
            CompilationScore = request.CompilationScore,
            TestsScore = request.TestsScore,
            QualityScore = request.QualityScore
        };
        _attemptService.CreateAttemptAsync(Arg.Any<RequestAttemptDto>())
            .Returns(Task.FromResult(expectedResponse));

        // Act
        var result = await _controller.Create(request);

        // Assert
        result.Result.Should().BeOfType<ObjectResult>()
            .Which.StatusCode.Should().Be(StatusCodes.Status201Created);
        result.Result.As<ObjectResult>().Value.Should().BeEquivalentTo(expectedResponse);
    }
    [Test]
    public async Task Delete_ReturnsOkResult_WithId()
    {
        // Arrange
        var id = Guid.NewGuid();
        _attemptService.DeleteAttemptAsync(id).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Delete(id);

        // Assert
        result.Result.Should().BeOfType<ObjectResult>()
            .Which.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Result.As<ObjectResult>().Value.Should().BeEquivalentTo(id);
    }
    [Test]
    public async Task Update_ReturnsOkResult_WithUpdatedAttempt()
    {
        // Arrange
        var id = Guid.NewGuid();
        var request = new RequestAttemptDto
        {
            StudentId = Guid.NewGuid(),
            AssignmentId = Guid.NewGuid(),
            BranchName = "Physics",
            AttemptNumber = 2,
            CompilationScore = 90,
            TestsScore = 92,
            QualityScore = 91,
        };
        var expectedResponse = new RespondAttemptDto
        {
            Id = id,
            StudentId = request.StudentId,
            AssignmentId = request.AssignmentId,
            BranchName = request.BranchName,
            AttemptNumber = request.AttemptNumber,
            CompilationScore = request.CompilationScore,
            TestsScore = request.TestsScore,
            QualityScore = request.QualityScore
        };
        _attemptService.UpdateAttemptAsync(id, request).Returns(Task.FromResult(expectedResponse));

        // Act
        var result = await _controller.Update(id, request);

        // Assert
        result.Should().BeOfType<ActionResult<RespondAttemptDto>>()
            .Which.Result.Should().BeOfType<ObjectResult>()
            .Which.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Result.As<ObjectResult>().Value.Should().BeEquivalentTo(expectedResponse);
    }
}