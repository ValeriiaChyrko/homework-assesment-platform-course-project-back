namespace HomeworkAssignment.Infrastructure.Abstractions;

public interface IGitHubClient
{
    Task<IEnumerable<string>> GetBranches(string owner, string repo, CancellationToken cancellationToken = default);

    Task<IEnumerable<string>> FilterBranchesByAuthor(string owner, string repo, IEnumerable<string> branches, string author,
        CancellationToken cancellationToken = default);
}