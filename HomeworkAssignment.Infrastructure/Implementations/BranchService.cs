using HomeworkAssignment.Infrastructure.Abstractions;
using Newtonsoft.Json.Linq;

namespace HomeworkAssignment.Infrastructure.Implementations;

public class BranchService : IBranchService
{
    private readonly IGitHubApiClient _gitHubApiApiClient;

    public BranchService(IGitHubApiClient gitHubApiApiClient)
    {
        _gitHubApiApiClient = gitHubApiApiClient ?? throw new ArgumentNullException(nameof(gitHubApiApiClient));
    }
    
    private static string BuildCommitsUrl(string owner, string repo, string branch, string author, DateTime? since, DateTime? until)
    {
        var url = $"repos/{owner}/{repo}/commits?sha={branch}&author={author}";

        if (since.HasValue) url += $"&since={since.Value:o}";
        if (until.HasValue) url += $"&until={until.Value:o}";

        return url;
    }

    public async Task<JArray> GetBranchesAsync(string owner, string repo, CancellationToken cancellationToken = default)
    {
        var url = $"repos/{owner}/{repo}/branches";
        var branchesJson = await _gitHubApiApiClient.GetJsonArrayAsync(url, cancellationToken);
        return branchesJson;
    }

    public async Task<IEnumerable<string>> FilterBranchesByAuthorAsync(string owner, string repo, IEnumerable<string> branches,
        string author, DateTime? since = null, DateTime? until = null, CancellationToken cancellationToken = default)
    {
        var studentBranches = new List<string>();

        foreach (var branch in branches)
        {
            var url = BuildCommitsUrl(owner, repo, branch, author, since, until);
            var commitList = await _gitHubApiApiClient.GetJsonArrayAsync(url, cancellationToken);

            if (commitList.Count > 0)
            {
                studentBranches.Add(branch);
            }
        }

        return studentBranches;
    }
}