using System.Net.Http.Headers;
using HomeworkAssignment.Infrastructure.Abstractions.GitHubRelated;
using Newtonsoft.Json.Linq;

namespace HomeworkAssignment.Infrastructure.Implementations.GitHubRelated;

public class GitHubApiApiClient : IGitHubApiClient
{
    private readonly HttpClient _httpClient;

    public GitHubApiApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _httpClient.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
    }

    public async Task<JArray> GetJsonArrayAsync(string url, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync(url, cancellationToken);
        response.EnsureSuccessStatusCode();

        var jsonResponse = await response.Content.ReadAsStringAsync(cancellationToken);
        return JArray.Parse(jsonResponse);
    }

    public async Task<JObject> GetJsonObjectAsync(string url, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync(url, cancellationToken);
        response.EnsureSuccessStatusCode();

        var jsonResponse = await response.Content.ReadAsStringAsync(cancellationToken);
        return JObject.Parse(jsonResponse);
    }
}