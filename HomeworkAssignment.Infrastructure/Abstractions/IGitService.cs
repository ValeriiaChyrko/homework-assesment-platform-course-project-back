namespace HomeworkAssignment.Infrastructure.Abstractions;

public interface IGitService
{
    void CloneRepository(string owner, string repositoryName, string targetDirectory);
    void CheckoutBranch(string repoDirectory, string branch);
    void CheckoutCommit(string repoDirectory, string commitSha);
}