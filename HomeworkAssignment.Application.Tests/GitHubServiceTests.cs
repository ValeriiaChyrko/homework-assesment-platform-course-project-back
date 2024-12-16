using FluentAssertions;
using HomeAssignment.Domain.Abstractions.Contracts;
using HomeAssignment.DTOs.RespondDTOs;
using HomeAssignment.DTOs.SharedDTOs;
using HomeworkAssignment.Application.Abstractions;
using HomeworkAssignment.Application.Implementations;
using HomeworkAssignment.Infrastructure.Abstractions.GitHubRelated;
using HomeworkAssignment.Infrastructure.Abstractions.GitRelated;
using Newtonsoft.Json.Linq;
using NSubstitute;

namespace HomeworkAssignment.Application.Tests;

[TestFixture]
public class GitHubServiceTests
{
    private IAssignmentService _assignmentService;
    private IBranchService _branchService;
    private ICommitService _commitService;
    private IGitHubBuildService _gitHubBuildService;
    private ILogger _logger;
    private IStudentService _studentService;
    private ITeacherService _teacherService;
    private GitHubService _gitHubService;

    [SetUp]
    public void SetUp()
    {
        _assignmentService = Substitute.For<IAssignmentService>();
        _branchService = Substitute.For<IBranchService>();
        _commitService = Substitute.For<ICommitService>();
        _gitHubBuildService = Substitute.For<IGitHubBuildService>();
        _logger = Substitute.For<ILogger>();
        _studentService = Substitute.For<IStudentService>();
        _teacherService = Substitute.For<ITeacherService>();

        _gitHubService = new GitHubService(
            _studentService,
            _logger,
            _assignmentService,
            _branchService,
            _teacherService,
            _gitHubBuildService,
            _commitService
        );
    }

    [Test]
    public async Task VerifyProjectCompilation_ShouldReturnScore_WhenProjectCompiles()
    {
        // Arrange
        var githubProfileId = Guid.NewGuid();
        var assignmentId = Guid.NewGuid();
        const string branch = "branch1";
        const string teacherUsername = "teacher123";
        const string repositoryName = "repo";
        const string commitSha = "123abc";

        var assignment = new RespondAssignmentDto
        {
            RepositoryName = repositoryName,
            OwnerId = Guid.NewGuid(),
            CompilationSection = new ScoreSectionDto { MaxScore = 100 }
        };
        var student = new RespondStudentDto { GithubUsername = "student123" };
        var teacher = new RespondTeacherDto { GithubUsername = teacherUsername };

        _assignmentService.GetAssignmentByIdAsync(assignmentId, Arg.Any<CancellationToken>()).Returns(assignment);
        _studentService.GetStudentByIdAsync(githubProfileId, Arg.Any<CancellationToken>()).Returns(student);
        _teacherService.GetTeacherByIdAsync(assignment.OwnerId, Arg.Any<CancellationToken>()).Returns(teacher);
        _commitService.GetLastCommitByAuthorAsync(teacherUsername, repositoryName, branch, student.GithubUsername, cancellationToken:Arg.Any<CancellationToken>()).Returns(commitSha);
        _gitHubBuildService.CheckIfProjectCompilesAsync(teacherUsername, repositoryName, branch, commitSha, cancellationToken:Arg.Any<CancellationToken>()).Returns(true);

        // Act
        var score = await _gitHubService.VerifyProjectCompilation(githubProfileId, assignmentId, branch);

        // Assert
        score.Should().Be(100);
    }
    [Test]
    public async Task VerifyProjectQuality_ShouldReturnScore_WhenProjectQualityEvaluated()
    {
        // Arrange
        var githubProfileId = Guid.NewGuid();
        var assignmentId = Guid.NewGuid();
        const string branch = "branch1";
        const string teacherUsername = "teacher123";
        const string repositoryName = "repo";
        const string commitSha = "123abc";

        var assignment = new RespondAssignmentDto
        {
            RepositoryName = repositoryName,
            OwnerId = Guid.NewGuid(),
            QualitySection = new ScoreSectionDto() { MaxScore = 100 }
        };
        var student = new RespondStudentDto { GithubUsername = "student123" };
        var teacher = new RespondTeacherDto { GithubUsername = teacherUsername };

        _assignmentService.GetAssignmentByIdAsync(assignmentId, Arg.Any<CancellationToken>()).Returns(assignment);
        _studentService.GetStudentByIdAsync(githubProfileId, Arg.Any<CancellationToken>()).Returns(student);
        _teacherService.GetTeacherByIdAsync(assignment.OwnerId, Arg.Any<CancellationToken>()).Returns(teacher);
        _commitService.GetLastCommitByAuthorAsync(teacherUsername, repositoryName, branch, student.GithubUsername, cancellationToken:Arg.Any<CancellationToken>()).Returns(commitSha);
        _gitHubBuildService.EvaluateProjectCodeQualityAsync(teacherUsername, repositoryName, branch, commitSha, cancellationToken:Arg.Any<CancellationToken>()).Returns(80);

        // Act
        var score = await _gitHubService.VerifyProjectQuality(githubProfileId, assignmentId, branch);

        // Assert
        score.Should().Be(80);
    }
    [Test]
    public async Task VerifyProjectTests_ShouldReturnScore_WhenTestsEvaluated()
    {
        // Arrange
        var githubProfileId = Guid.NewGuid();
        var assignmentId = Guid.NewGuid();
        const string branch = "branch1";
        const string teacherUsername = "teacher123";
        const string repositoryName = "repo";
        const string commitSha = "123abc";

        var assignment = new RespondAssignmentDto
        {
            RepositoryName = repositoryName,
            OwnerId = Guid.NewGuid(),
            TestsSection = new ScoreSectionDto { MaxScore = 100 }
        };
        var student = new RespondStudentDto { GithubUsername = "student123" };
        var teacher = new RespondTeacherDto { GithubUsername = teacherUsername };

        _assignmentService.GetAssignmentByIdAsync(assignmentId, Arg.Any<CancellationToken>()).Returns(assignment);
        _studentService.GetStudentByIdAsync(githubProfileId, Arg.Any<CancellationToken>()).Returns(student);
        _teacherService.GetTeacherByIdAsync(assignment.OwnerId, Arg.Any<CancellationToken>()).Returns(teacher);
        _commitService.GetLastCommitByAuthorAsync(teacherUsername, repositoryName, branch, student.GithubUsername, cancellationToken:Arg.Any<CancellationToken>()).Returns(commitSha);
        _gitHubBuildService.EvaluateProjectCodePassedTestsAsync(teacherUsername, repositoryName, branch, commitSha, cancellationToken:Arg.Any<CancellationToken>()).Returns(70);

        // Act
        var score = await _gitHubService.VerifyProjectTests(githubProfileId, assignmentId, branch);

        // Assert
        score.Should().Be(70);
    }
}