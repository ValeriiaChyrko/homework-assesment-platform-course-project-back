using Newtonsoft.Json.Linq;

namespace HomeworkAssignment.Infrastructure.Abstractions.GitHubRelated;

public interface IGitHubApiClient
{
    Task<JArray> GetJsonArrayAsync(string url, CancellationToken cancellationToken = default);
    Task<JObject> GetJsonObjectAsync(string url, CancellationToken cancellationToken = default);
}