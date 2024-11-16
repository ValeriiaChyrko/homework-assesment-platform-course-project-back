namespace HomeworkAssignment.Infrastructure.Abstractions.GitRelated;

public interface IGitService
{
    void CloneRepository(string owner, string repositoryName, string targetDirectory);
    void CheckoutBranch(string repoDirectory, string branch);
    void CheckoutCommit(string repoDirectory, string commitSha);
}