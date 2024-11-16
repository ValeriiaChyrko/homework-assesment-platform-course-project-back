using Newtonsoft.Json.Linq;

namespace HomeworkAssignment.Infrastructure.Abstractions.GitRelated;

public interface IBranchService
{
    Task<JArray> GetBranchesAsync(string owner, string repo, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<string>> GetBranchesWithCommitsByAuthorAsync(string owner, string repo,
        IEnumerable<string> branches,
        string? author, DateTime? since = null, DateTime? until = null, CancellationToken cancellationToken = default);
}