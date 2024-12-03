using HomeAssignment.Domain.Abstractions.Contracts;
using HomeAssignment.DTOs.RespondDTOs;
using HomeworkAssignment.Application.Abstractions;
using HomeworkAssignment.Application.Common;
using HomeworkAssignment.Infrastructure.Abstractions.GitHubRelated;
using HomeworkAssignment.Infrastructure.Abstractions.GitRelated;

namespace HomeworkAssignment.Application.Implementations;

public class GitHubService : IGitHubService
{
    private readonly IAssignmentService _assignmentService;
    private readonly IBranchService _branchService;
    private readonly ICommitService _commitService;
    private readonly IGitHubBuildService _gitHubBuildService;
    private readonly ILogger _logger;
    private readonly IStudentService _studentService;
    private readonly ITeacherService _teacherService;

    public GitHubService(
        IStudentService studentService,
        ILogger logger,
        IAssignmentService assignmentService,
        IBranchService branchService,
        ITeacherService teacherService,
        IGitHubBuildService gitHubBuildService,
        ICommitService commitService)
    {
        _studentService = studentService;
        _logger = logger;
        _assignmentService = assignmentService;
        _branchService = branchService;
        _teacherService = teacherService;
        _gitHubBuildService = gitHubBuildService;
        _commitService = commitService;
    }

    public async Task<IEnumerable<string>?> GetStudentBranches(Guid githubProfileId, Guid assignmentId, CancellationToken cancellationToken = default)
    {
        try
        {
            var (assignment, student, teacher) = await GetEntitiesAsync(githubProfileId, assignmentId, cancellationToken);
            if (assignment == null || student == null || teacher == null) return null;

            var branches = await _branchService.GetBranchesAsync(teacher.GithubUsername, assignment.RepositoryName, cancellationToken);
            var branchNames = branches.Select(b => b["name"]?.ToString()!).Where(name => !string.IsNullOrEmpty(name));

            return await _branchService.GetBranchesWithCommitsByAuthorAsync(
                teacher.GithubUsername,
                assignment.RepositoryName,
                branchNames,
                student.GithubUsername,
                cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            LogError("Error getting GitHub branches", ex);
            throw;
        }
    }

    public async Task<int> VerifyProjectCompilation(Guid githubProfileId, Guid assignmentId, string branch, CancellationToken cancellationToken = default)
        => await VerifyProjectScoreAsync(githubProfileId, assignmentId, branch, SectionType.Compilation, cancellationToken);

    public async Task<int> VerifyProjectQuality(Guid githubProfileId, Guid assignmentId, string branch, CancellationToken cancellationToken = default)
        => await VerifyProjectScoreAsync(githubProfileId, assignmentId, branch, SectionType.Quality, cancellationToken);

    public async Task<int> VerifyProjectTests(Guid githubProfileId, Guid assignmentId, string branch, CancellationToken cancellationToken = default)
        => await VerifyProjectScoreAsync(githubProfileId, assignmentId, branch, SectionType.Tests, cancellationToken);

    private async Task<(RespondAssignmentDto?, RespondStudentDto?, RespondTeacherDto?)> GetEntitiesAsync(Guid githubProfileId, Guid assignmentId, CancellationToken cancellationToken)
    {
        var assignment = await _assignmentService.GetAssignmentByIdAsync(assignmentId, cancellationToken);
        var student = await _studentService.GetStudentByIdAsync(githubProfileId, cancellationToken);
        var teacher = await _teacherService.GetTeacherByIdAsync(githubProfileId, cancellationToken);
        return (assignment, student, teacher);
    }

    private async Task<int> VerifyProjectScoreAsync(Guid githubProfileId, Guid assignmentId, string branch, SectionType sectionType, CancellationToken cancellationToken)
    {
        try
        {
            var (assignment, student, teacher) = await GetEntitiesAsync(githubProfileId, assignmentId, cancellationToken);
            if (assignment == null || student == null || teacher == null) return 0;

            var section = assignment.GetSection(sectionType);
            if (section == null) return 0;

            var lastCommitSha = await _commitService.GetLastCommitByAuthorAsync(
                teacher.GithubUsername,
                assignment.RepositoryName,
                branch,
                student.GithubUsername,
                cancellationToken:cancellationToken);

            if (string.IsNullOrEmpty(lastCommitSha))
            {
                _logger.Log($"No commits found for {sectionType} verification.");
                return 0;
            }

            var percentage = sectionType switch
            {
                SectionType.Compilation => await _gitHubBuildService.CheckIfProjectCompilesAsync(teacher.GithubUsername, assignment.RepositoryName, branch, lastCommitSha, cancellationToken) ? 100 : 0,
                SectionType.Quality => await _gitHubBuildService.EvaluateProjectCodeQualityAsync(teacher.GithubUsername, assignment.RepositoryName, branch, lastCommitSha, cancellationToken),
                SectionType.Tests => await _gitHubBuildService.EvaluateProjectCodePassedTestsAsync(teacher.GithubUsername, assignment.RepositoryName, branch, lastCommitSha, cancellationToken),
                _ => 0
            };

            return CalculateScore(section.MinScore, section.MaxScore, percentage);
        }
        catch (Exception ex)
        {
            LogError($"Error verifying project {sectionType.ToString().ToLower()} score", ex);
            throw;
        }
    }

    private static int CalculateScore(int minScore, int maxScore, int percentage)
    {
        if (minScore == maxScore) return minScore;
        return (int)Math.Round(minScore + percentage / 100.0 * (maxScore - minScore));
    }

    private void LogError(string message, Exception ex)
    {
        _logger.Log($"{message}: {ex.Message}.");
    }
}