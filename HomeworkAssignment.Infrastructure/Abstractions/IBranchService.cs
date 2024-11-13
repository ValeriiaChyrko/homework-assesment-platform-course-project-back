using Newtonsoft.Json.Linq;

namespace HomeworkAssignment.Infrastructure.Abstractions;

public interface IBranchService
{
    Task<JArray> GetBranchesAsync(string owner, string repo, CancellationToken cancellationToken = default);
    Task<IEnumerable<string>> FilterBranchesByAuthorAsync(string owner, string repo, IEnumerable<string> branches,
        string author, DateTime? since = null, DateTime? until = null, CancellationToken cancellationToken = default);
}