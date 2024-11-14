namespace HomeworkAssignment.Infrastructure.Abstractions;

public interface IGitHubBuildService
{
    Task<bool> CheckIfProjectCompilesAsync(string owner, string repo, string branch,
        string lastCommitSha, CancellationToken cancellationToken = default);

    Task<int> EvaluateProjectCodeQualityAsync(string owner, string repositoryName, string branch, string lastCommitSha,
        CancellationToken cancellationToken = default);
}