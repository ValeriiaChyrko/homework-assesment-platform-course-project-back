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
public class TeacherControllerTests
{
    private ITeacherService _teacherService;
    private TeacherController _controller;

    [SetUp]
    public void SetUp()
    {
        _teacherService = Substitute.For<ITeacherService>();
        _controller = new TeacherController(_teacherService);
    }

    [Test]
    public async Task Get_ReturnsOkResult_WithTeachers()
    {
        // Arrange
        var mockTeachers = new List<RespondTeacherDto>
        {
            new()
            {
                UserId = Guid.NewGuid(),
                GitHubProfileId = Guid.NewGuid(),
                FullName = "Mr. Smith",
                Email = "mr.smith@example.com",
                Password = "password123",
                GithubUsername = "mrsmith",
                GithubProfileUrl = "https://github.com/mrsmith",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        };
        _teacherService.GetTeachersAsync().Returns(Task.FromResult<IReadOnlyList<RespondTeacherDto>>(mockTeachers));

        // Act
        var result = await _controller.Get();

        // Assert
        result.Result.Should().BeOfType<ObjectResult>()
            .Which.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Result.As<ObjectResult>().Value.Should().BeEquivalentTo(mockTeachers);
    }
    [Test]
    public async Task Get_WithValidId_ReturnsOkResult_WithTeacher()
    {
        // Arrange
        var githubProfileId = Guid.NewGuid();
        var expectedTeacher = new RespondTeacherDto
        {
            UserId = Guid.NewGuid(),
            GitHubProfileId = githubProfileId,
            FullName = "Mrs. Johnson",
            Email = "mrs.johnson@example.com",
            Password = "password456",
            GithubUsername = "mrsjohnson",
            GithubProfileUrl = "https://github.com/mrsjohnson",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        _teacherService.GetTeacherByIdAsync(githubProfileId)!.Returns(Task.FromResult(expectedTeacher));

        // Act
        var result = await _controller.Get(githubProfileId);

        // Assert
        result.Should().BeOfType<ActionResult<RespondTeacherDto>>()
            .Which.Result.Should().BeOfType<ObjectResult>()
            .Which.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Result.As<ObjectResult>().Value.Should().BeEquivalentTo(expectedTeacher);
    }
    [Test]
    public async Task Get_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        var githubProfileId = Guid.NewGuid();
        _teacherService.GetTeacherByIdAsync(githubProfileId)!.Returns(Task.FromResult<RespondTeacherDto>(null!));

        // Act
        var result = await _controller.Get(githubProfileId);

        // Assert
        result.Should().BeOfType<ActionResult<RespondTeacherDto>>()
            .Which.Result.Should().BeOfType<StatusCodeResult>()
            .Which.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }
    [Test]
    public async Task Create_ReturnsCreatedResult_WithCreatedTeacher()
    {
        // Arrange
        var request = new RequestTeacherDto
        {
            FullName = "Alice Walker",
            Email = "alice.walker@example.com",
            Password = "password789",
            GithubUsername = "alicewalker",
            GithubProfileUrl = "https://github.com/alicewalker",
            GithubPictureUrl = "https://github.com/alicewalker/photo"
        };
        var expected = new RespondTeacherDto
        {
            UserId = Guid.NewGuid(),
            GitHubProfileId = Guid.NewGuid(),
            FullName = "Alice Walker",
            Email = "alice.walker@example.com",
            Password = "password789",
            GithubUsername = "alicewalker",
            GithubProfileUrl = "https://github.com/alicewalker",
            GithubPictureUrl = "https://github.com/alicewalker/photo"
        };
        _teacherService.CreateTeacherAsync(request).Returns(Task.FromResult(expected));

        // Act
        var result = await _controller.Create(request);

        // Assert
        result.Result.Should().BeOfType<ObjectResult>()
            .Which.StatusCode.Should().Be(StatusCodes.Status201Created);
        result.Result.As<ObjectResult>().Value.Should().Be(expected);
    }
    [Test]
    public async Task Delete_ReturnsOkResult_WithUser_Id()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _teacherService.DeleteTeacherAsync(userId).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Delete(userId);

        // Assert
        result.Result.Should().BeOfType<ObjectResult>()
            .Which.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Result.As<ObjectResult>().Value.Should().BeEquivalentTo(userId);
    }
    [Test]
    public async Task Update_ReturnsOkResult_WithUpdatedTeacher()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var githubProfileId = Guid.NewGuid();
        var request = new RequestTeacherDto
        {
            FullName = "Bob Brown",
            Email = "bob.brown@example.com",
            Password = "newpassword123",
            GithubUsername = "bobbrown",
            GithubProfileUrl = "https://github.com/bobbrown",
            GithubPictureUrl = "https://github.com/alicewalker/photo"
        };
        var expectedResponse = new RespondTeacherDto
        {
            UserId = userId,
            GitHubProfileId = githubProfileId,
            FullName = request.FullName,
            Email = request.Email,
            Password = request.Password,
            GithubUsername = request.GithubUsername,
            GithubProfileUrl = request.GithubProfileUrl,
            GithubPictureUrl = request.GithubPictureUrl,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        _teacherService.UpdateTeacherAsync(userId, githubProfileId, request).Returns(Task.FromResult(expectedResponse));

        // Act
        var result = await _controller.Update(userId, githubProfileId, request);

        // Assert
        result.Should().BeOfType<ActionResult<RespondTeacherDto>>()
            .Which.Result.Should().BeOfType<ObjectResult>()
            .Which.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Result.As<ObjectResult>().Value.Should().BeEquivalentTo(expectedResponse);
    }
}