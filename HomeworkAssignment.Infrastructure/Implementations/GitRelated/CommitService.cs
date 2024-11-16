using HomeworkAssignment.Infrastructure.Abstractions.GitHubRelated;
using HomeworkAssignment.Infrastructure.Abstractions.GitRelated;
using Newtonsoft.Json.Linq;

namespace HomeworkAssignment.Infrastructure.Implementations.GitRelated;

public class CommitService : ICommitService
{
    private readonly IGitHubApiClient _gitHubApiApiClient;

    public CommitService(IGitHubApiClient gitHubApiApiClient)
    {
        _gitHubApiApiClient = gitHubApiApiClient ?? throw new ArgumentNullException(nameof(gitHubApiApiClient));
    }

    public async Task<JArray> GetCommitsForBranchAsync(string owner, string repo, string branch,
        CancellationToken cancellationToken = default)
    {
        var url = $"repos/{owner}/{repo}/commits?sha={branch}";
        var commitListJson = await _gitHubApiApiClient.GetJsonArrayAsync(url, cancellationToken);
        return commitListJson;
    }

    public async Task<IEnumerable<string?>> FilterCommitsByAuthorAsync(string owner, string repo, string branch,
        string author, DateTime? since = null, DateTime? until = null, CancellationToken cancellationToken = default)
    {
        var url = BuildCommitsUrl(owner, repo, branch, author, since, until);
        var commitListJson = await _gitHubApiApiClient.GetJsonArrayAsync(url, cancellationToken);
        return commitListJson.Select(commit => commit["sha"]?.ToString()).ToList();
    }

    public async Task<string?> GetLastCommitByAuthorAsync(string owner, string repo, string branch,
        string author, DateTime? since = null, DateTime? until = null, CancellationToken cancellationToken = default)
    {
        var url = BuildCommitsUrl(owner, repo, branch, author, since, until) + "&per_page=1";
        var commitListJson = await _gitHubApiApiClient.GetJsonArrayAsync(url, cancellationToken);
        return commitListJson.Count > 0 ? commitListJson[0]["sha"]?.ToString() : null;
    }

    private static string BuildCommitsUrl(string owner, string repo, string branch, string author, DateTime? since,
        DateTime? until)
    {
        var url = $"repos/{owner}/{repo}/commits?sha={branch}&author={author}";

        if (since.HasValue) url += $"&since={since.Value:o}";
        if (until.HasValue) url += $"&until={until.Value:o}";

        return url;
    }
}