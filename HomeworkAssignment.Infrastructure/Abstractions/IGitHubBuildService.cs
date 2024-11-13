namespace HomeworkAssignment.Infrastructure.Abstractions;

public interface IGitHubBuildService
{
    Task<bool> CheckIfProjectCompiles(string owner, string repo, string branch,
        string lastCommitSha, CancellationToken cancellationToken = default);
}