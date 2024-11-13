using Newtonsoft.Json.Linq;

namespace HomeworkAssignment.Infrastructure.Abstractions;

public interface ICommitService
{
    Task<JArray> GetCommitsForBranchAsync(string owner, string repo, string branch,
        CancellationToken cancellationToken = default);
    Task<IEnumerable<string?>> FilterCommitsByAuthorAsync(string owner, string repo, string branch,
        string author, DateTime? since = null, DateTime? until = null, CancellationToken cancellationToken = default);
    Task<string?> GetLastCommitByAuthorAsync(string owner, string repo, string branch,
        string author, DateTime? since = null, DateTime? until = null, CancellationToken cancellationToken = default);
}