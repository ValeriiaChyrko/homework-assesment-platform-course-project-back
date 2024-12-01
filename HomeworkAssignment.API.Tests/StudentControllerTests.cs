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
public class StudentControllerTests
{
    private IStudentService _studentService;
    private StudentController _controller;

    [SetUp]
    public void SetUp()
    {
        _studentService = Substitute.For<IStudentService>();
        _controller = new StudentController(_studentService);
    }

    [Test]
    public async Task Get_ReturnsOkResult_WithStudents()
    {
        // Arrange
        var mockStudents = new List<RespondStudentDto>
        {
            new()
            {
                UserId = Guid.NewGuid(),
                GitHubProfileId = Guid.NewGuid(),
                FullName = "John Doe",
                Email = "john.doe@example.com",
                Password = "password123",
                GithubUsername = "johndoe",
                GithubProfileUrl = "https://github.com/johndoe",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        };
        _studentService.GetStudentsAsync().Returns(Task.FromResult<IReadOnlyList<RespondStudentDto>>(mockStudents));

        // Act
        var result = await _controller.Get();

        // Assert
        result.Result.Should().BeOfType<ObjectResult>()
            .Which.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Result.As<ObjectResult>().Value.Should().BeEquivalentTo(mockStudents);
    }
    [Test]
    public async Task Get_WithValidId_ReturnsOkResult_WithStudent()
    {
        // Arrange
        var githubProfileId = Guid.NewGuid();
        var expectedStudent = new RespondStudentDto
        {
            UserId = Guid.NewGuid(),
            GitHubProfileId = githubProfileId,
            FullName = "Jane Doe",
            Email = "jane.doe@example.com",
            Password = "password456",
            GithubUsername = "janedoe",
            GithubProfileUrl = "https://github.com/janedoe",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        _studentService.GetStudentByIdAsync(githubProfileId)!.Returns(Task.FromResult(expectedStudent));

        // Act
        var result = await _controller.Get(githubProfileId);

        // Assert
        result.Should().BeOfType<ActionResult<RespondStudentDto>>()
            .Which.Result.Should().BeOfType<ObjectResult>()
            .Which.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Result.As<ObjectResult>().Value.Should().BeEquivalentTo(expectedStudent);
    }
    [Test]
    public async Task Get_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        var githubProfileId = Guid.NewGuid();
        _studentService.GetStudentByIdAsync(githubProfileId)!.Returns(Task.FromResult<RespondStudentDto>(null!));

        // Act
        var result = await _controller.Get(githubProfileId);

        // Assert
        result.Should().BeOfType<ActionResult<RespondStudentDto>>()
            .Which.Result.Should().BeOfType<StatusCodeResult>()
            .Which.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }
    [Test]
    public async Task Create_ReturnsCreatedResult_WithCreatedStudent()
    {
        // Arrange
        var request = new RequestStudentDto
        {
            FullName = "Alice Smith",
            Email = "alice.smith@example.com",
            Password = "password789",
            GithubUsername = "alicesmith",
            GithubProfileUrl = "https://github.com/alicesmith",
            GithubPictureUrl = "https://github.com/alicesmith/photo"
        };
        var expected = new RespondStudentDto
        {
            UserId = Guid.NewGuid(),
            GitHubProfileId = Guid.NewGuid(),
            FullName = "Alice Smith",
            Email = "alice.smith@example.com",
            Password = "password789",
            GithubUsername = "alicesmith",
            GithubProfileUrl = "https://github.com/alicesmith",
            GithubPictureUrl = "https://github.com/alicesmith/photo"
        };
        _studentService.CreateStudentAsync(request).Returns(Task.FromResult(expected));

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
        _studentService.DeleteStudentAsync(userId).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Delete(userId);

        // Assert
        result.Result.Should().BeOfType<ObjectResult>()
            .Which.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Result.As<ObjectResult>().Value.Should().BeEquivalentTo(userId);
    }
    [Test]
    public async Task Update_ReturnsOkResult_WithUpdatedStudent()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var githubProfileId = Guid.NewGuid();
        var request = new RequestStudentDto
        {
            FullName = "Bob Johnson",
            Email = "bob.johnson@example.com",
            Password = "newpassword123",
            GithubUsername = "bobjohnson",
            GithubProfileUrl = "https://github.com/bobjohnson",
            GithubPictureUrl = "https://github.com/bobjohnson/photo"
        };
        var expectedResponse = new RespondStudentDto
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
        _studentService.UpdateStudentAsync(userId, githubProfileId, request).Returns(Task.FromResult(expectedResponse));

        // Act
        var result = await _controller.Update(userId, githubProfileId, request);

        // Assert
        result.Should().BeOfType<ActionResult<RespondStudentDto>>()
            .Which.Result.Should().BeOfType<ObjectResult>()
            .Which.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Result.As<ObjectResult>().Value.Should().BeEquivalentTo(expectedResponse);
    }
}