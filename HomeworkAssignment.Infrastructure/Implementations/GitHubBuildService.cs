using HomeworkAssignment.Infrastructure.Abstractions;

namespace HomeworkAssignment.Infrastructure.Implementations;

public class GitHubBuildService : IGitHubBuildService
{
    private readonly IGitService _gitService;
    private readonly IBuildService _buildService;

    public GitHubBuildService(IGitService gitService, IBuildService buildService)
    {
        _gitService = gitService;
        _buildService = buildService;
    }

    public async Task<bool> CheckIfProjectCompiles(string owner, string repositoryName, string branch, string lastCommitSha, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(lastCommitSha)) return false;

        var repoDirectory = Path.Combine(Path.GetTempPath(), string.Join('_', repositoryName, Guid.NewGuid()));
        _gitService.CloneRepository(owner, repositoryName, repoDirectory);
        _gitService.CheckoutBranch(repoDirectory, branch); 
        _gitService.CheckoutCommit(repoDirectory, lastCommitSha); 

        var projectFiles = Directory.GetFiles(repoDirectory, "*.csproj", SearchOption.AllDirectories);
        if (projectFiles.Length == 0) return false;

        var overallSuccess = true;

        foreach (var projectFile in projectFiles)
        {
            var buildSuccess = await _buildService.BuildProject(projectFile);
            if (!buildSuccess)
            {
                overallSuccess = false;
            }
        }

        return overallSuccess;
    }
}