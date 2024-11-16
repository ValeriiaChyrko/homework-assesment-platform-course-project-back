using HomeAssignment.Domain.Abstractions.Contracts;
using HomeworkAssignment.Infrastructure.Abstractions.CompilationSection;
using HomeworkAssignment.Infrastructure.Abstractions.GitHubRelated;
using HomeworkAssignment.Infrastructure.Abstractions.GitRelated;
using HomeworkAssignment.Infrastructure.Abstractions.QualitySection;
using HomeworkAssignment.Infrastructure.Abstractions.TestsSection;

namespace HomeworkAssignment.Infrastructure.Implementations.GitHubRelated;

public class GitHubBuildService : IGitHubBuildService
{
    private readonly ICodeBuildService _codeBuildService;
    private readonly ICodeTestsService _codeTestsService;
    private readonly IGitService _gitService;
    private readonly ILogger _logger;
    private readonly ICodeQualityService _qualityService;

    public GitHubBuildService(IGitService gitService, ICodeBuildService codeBuildService,
        ICodeQualityService qualityService, ILogger logger, ICodeTestsService codeTestsService)
    {
        _gitService = gitService;
        _codeBuildService = codeBuildService;
        _qualityService = qualityService;
        _logger = logger;
        _codeTestsService = codeTestsService;
    }

    public async Task<bool> CheckIfProjectCompilesAsync(string owner, string repositoryName, string branch,
        string lastCommitSha, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(lastCommitSha)) return false;

        var repoDirectory = Path.Combine(Path.GetTempPath(), repositoryName);
        _gitService.CloneRepository(owner, repositoryName, repoDirectory);
        _gitService.CheckoutBranch(repoDirectory, branch);
        _gitService.CheckoutCommit(repoDirectory, lastCommitSha);

        var projectFiles = Directory.GetFiles(repoDirectory, "*.csproj", SearchOption.AllDirectories);
        if (projectFiles.Length == 0) return false;

        var overallSuccess = true;

        foreach (var projectFile in projectFiles)
            try
            {
                var exitCode = await _codeBuildService.BuildProjectAsync(projectFile, cancellationToken);
                if (exitCode != 0) overallSuccess = false;
            }
            catch (Exception ex)
            {
                _logger.Log($"Error building project {projectFile}: {ex.Message}");
                overallSuccess = false;
            }

        return overallSuccess;
    }

    public async Task<int> EvaluateProjectCodeQualityAsync(string owner, string repositoryName, string branch,
        string lastCommitSha, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(lastCommitSha)) return 0;

        var repoDirectory = Path.Combine(Path.GetTempPath(), repositoryName);
        if (!Directory.Exists(repoDirectory)) _gitService.CloneRepository(owner, repositoryName, repoDirectory);

        _gitService.CheckoutBranch(repoDirectory, branch);
        _gitService.CheckoutCommit(repoDirectory, lastCommitSha);

        var score = await _qualityService.CheckCodeQualityAsync(repoDirectory, cancellationToken);
        return score;
    }

    public async Task<int> EvaluateProjectCodePassedTestsAsync(string owner, string repositoryName, string branch,
        string lastCommitSha, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(lastCommitSha)) return 0;

        var repoDirectory = Path.Combine(Path.GetTempPath(), repositoryName);
        if (!Directory.Exists(repoDirectory)) _gitService.CloneRepository(owner, repositoryName, repoDirectory);

        _gitService.CheckoutBranch(repoDirectory, branch);
        _gitService.CheckoutCommit(repoDirectory, lastCommitSha);

        var score = await _codeTestsService.CheckCodeTestsAsync(repoDirectory, cancellationToken);
        return score;
    }
}