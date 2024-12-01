using FluentAssertions;
using HomeworkAssignment.Application.Abstractions;
using HomeworkAssignment.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;

namespace HomeworkAssignment.API.Tests;

[TestFixture]
public class GitHubApiControllerTests
{
    private IGitHubService _gitHubService;
    private GitHubApiController _controller;

    [SetUp]
    public void SetUp()
    {
        _gitHubService = Substitute.For<IGitHubService>();
        _controller = new GitHubApiController(_gitHubService);
    }

    [Test]
    public async Task Get_ReturnsOkResult_WithBranches()
    {
        // Arrange
        var githubProfileId = Guid.NewGuid();
        var assignmentId = Guid.NewGuid();
        var mockBranches = new List<string> { "main", "develop", "feature" };
        
        _gitHubService.GetStudentBranches(githubProfileId, assignmentId)!
            .Returns(Task.FromResult<IEnumerable<string>>(mockBranches));

        // Act
        var result = await _controller.Get(githubProfileId, assignmentId);

        // Assert
        result.Should().BeOfType<ActionResult<IReadOnlyList<string>>>()
            .Which.Result.Should().BeOfType<ObjectResult>()
            .Which.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Result.As<ObjectResult>().Value.Should().BeEquivalentTo(mockBranches);
    }
    [Test]
    public async Task GetProjectCompilationVerification_ReturnsOkResult_WithCompilationStatus()
    {
        // Arrange
        var githubProfileId = Guid.NewGuid();
        var assignmentId = Guid.NewGuid();
        const string branch = "main";
        const int mockCompilationStatus = 1; 
        _gitHubService.VerifyProjectCompilation(githubProfileId, assignmentId, branch)
            .Returns(Task.FromResult(mockCompilationStatus));

        // Act
        var result = await _controller.GetProjectCompilationVerification(githubProfileId, assignmentId, branch);

        // Assert
        result.Should().BeOfType<ActionResult<int>>()
            .Which.Result.Should().BeOfType<ObjectResult>()
            .Which.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Result.As<ObjectResult>().Value.Should().Be(mockCompilationStatus);
    }
    [Test]
    public async Task GetProjectQualityVerification_ReturnsOkResult_WithQualityStatus()
    {
        // Arrange
        var githubProfileId = Guid.NewGuid();
        var assignmentId = Guid.NewGuid();
        const string branch = "develop";
        const int mockQualityStatus = 2; 
        _gitHubService.VerifyProjectQuality(githubProfileId, assignmentId, branch)
            .Returns(Task.FromResult(mockQualityStatus));

        // Act
        var result = await _controller.GetProjectQualityVerification(githubProfileId, assignmentId, branch);

        // Assert
        result.Should().BeOfType<ActionResult<int>>()
            .Which.Result.Should().BeOfType<ObjectResult>()
            .Which.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Result.As<ObjectResult>().Value.Should().Be(mockQualityStatus);
    }
    [Test]
    public async Task GetProjectTestsVerification_ReturnsOkResult_WithTestsStatus()
    {
        // Arrange
        var githubProfileId = Guid.NewGuid();
        var assignmentId = Guid.NewGuid();
        const string branch = "feature";
        const int mockTestsStatus = 3; 
        _gitHubService.VerifyProjectTests(githubProfileId, assignmentId, branch)
            .Returns(Task.FromResult(mockTestsStatus));

        // Act
        var result = await _controller.GetProjectTestsVerification(githubProfileId, assignmentId, branch);

        // Assert
        result.Should().BeOfType<ActionResult<int>>()
            .Which.Result.Should().BeOfType<ObjectResult>()
            .Which.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Result.As<ObjectResult>().Value.Should().Be(mockTestsStatus);
    }
}