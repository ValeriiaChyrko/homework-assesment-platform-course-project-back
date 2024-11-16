using HomeworkAssignment.Infrastructure.Abstractions.GitHubRelated;
using HomeworkAssignment.Infrastructure.Abstractions.GitRelated;
using Newtonsoft.Json.Linq;

namespace HomeworkAssignment.Infrastructure.Implementations.GitRelated;

public class BranchService : IBranchService
{
    private readonly IGitHubApiClient _gitHubApiClient;

    public BranchService(IGitHubApiClient gitHubApiClient)
    {
        _gitHubApiClient = gitHubApiClient ?? throw new ArgumentNullException(nameof(gitHubApiClient));
    }

    public async Task<JArray> GetBranchesAsync(string owner, string repo,
        CancellationToken cancellationToken = default)
    {
        var url = $"repos/{owner}/{repo}/branches";
        return await _gitHubApiClient.GetJsonArrayAsync(url, cancellationToken);
    }

    public async Task<IReadOnlyCollection<string>> GetBranchesWithCommitsByAuthorAsync(string owner, string repo,
        IEnumerable<string> branches,
        string? author, DateTime? since = null, DateTime? until = null,
        CancellationToken cancellationToken = default)
    {
        var studentBranches = new HashSet<string>();

        foreach (var branch in branches)
        {
            var url = BuildCommitsUrl(owner, repo, branch, author, since, until);
            var commitList = await _gitHubApiClient.GetJsonArrayAsync(url, cancellationToken);

            if (commitList.Count > 0) studentBranches.Add(branch);
        }

        return studentBranches;
    }

    private static string BuildCommitsUrl(string owner, string repo, string branch, string? author = null,
        DateTime? since = null, DateTime? until = null)
    {
        var url = $"repos/{owner}/{repo}/commits?sha={branch}";

        if (!string.IsNullOrEmpty(author)) url += $"&author={author}";

        if (since.HasValue) url += $"&since={since.Value:o}";

        if (until.HasValue) url += $"&until={until.Value:o}";

        return url;
    }
}