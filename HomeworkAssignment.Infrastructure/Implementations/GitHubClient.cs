using HomeworkAssignment.Infrastructure.Abstractions;
using Newtonsoft.Json.Linq;

namespace HomeworkAssignment.Infrastructure.Implementations;

public class GitHubClient : IGitHubClient
{
    private readonly HttpClient _httpClient;

    public GitHubClient(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    public async Task<IEnumerable<string>> GetBranches(string owner, string repo,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetStringAsync($"repos/{owner}/{repo}/branches", cancellationToken);
        var branches = JArray.Parse(response);
        return branches.Select(branch => branch["name"]?.ToString())!;
    }

    public async Task<IEnumerable<string>> FilterBranchesByAuthor(string owner, string repo, IEnumerable<string> branches,
        string author, CancellationToken cancellationToken = default)
    {
        var studentBranches = new List<string>();

        foreach (var branch in branches)
        {
            var commits = await _httpClient.GetStringAsync($"repos/{owner}/{repo}/commits?sha={branch}&author={author}",
                cancellationToken);
            var commitList = JArray.Parse(commits);

            if (commitList.Count > 0) studentBranches.Add(branch);
        }

        return studentBranches;
    }
}