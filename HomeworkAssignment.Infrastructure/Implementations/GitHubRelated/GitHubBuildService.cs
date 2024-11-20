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
    private readonly ICodeQualityService _qualityService;

    public GitHubBuildService(IGitService gitService, ICodeBuildService codeBuildService,
        ICodeQualityService qualityService, ICodeTestsService codeTestsService)
    {
        _gitService = gitService;
        _codeBuildService = codeBuildService;
        _qualityService = qualityService;
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

        var isCompile = await _codeBuildService.VerifyProjectCompilation(repoDirectory, cancellationToken);
        return isCompile;
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