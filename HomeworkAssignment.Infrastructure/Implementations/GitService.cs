using HomeworkAssignment.Infrastructure.Abstractions;
using LibGit2Sharp;

namespace HomeworkAssignment.Infrastructure.Implementations;

public class GitService : IGitService
{
    public void CloneRepository(string owner, string repositoryName, string targetDirectory)
    {
        if (Directory.Exists(targetDirectory))
        {
            Directory.Delete(targetDirectory, true);
        }
        Repository.Clone($"https://github.com/{owner}/{repositoryName}.git", targetDirectory);
    }

    public void CheckoutBranch(string repoDirectory, string branch)
    {
        using var repo = new Repository(repoDirectory);
        Commands.Checkout(repo, branch);
    }

    public void CheckoutCommit(string repoDirectory, string commitSha)
    {
        using var repo = new Repository(repoDirectory);
        var commit = repo.Commits.FirstOrDefault(c => c.Sha.Equals(commitSha, StringComparison.OrdinalIgnoreCase));
        if (commit == null) throw new Exception("Commit not found.");
        Commands.Checkout(repo, commit);
    }
}