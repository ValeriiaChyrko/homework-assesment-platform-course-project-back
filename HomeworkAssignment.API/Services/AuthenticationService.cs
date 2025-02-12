using HomeworkAssignment.Services.Abstractions;
using Newtonsoft.Json;

namespace HomeworkAssignment.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly HttpClient _httpClient;
    private readonly string _tokenEndpoint;
    private readonly string _clientId;
    private readonly string _clientSecret;

    public AuthenticationService(HttpClient httpClient, string tokenEndpoint, string clientId, string clientSecret)
    {
        _httpClient = httpClient;
        _tokenEndpoint = tokenEndpoint;
        _clientId = clientId;
        _clientSecret = clientSecret;
    }

    public async Task<string?> GetAccessTokenAsync(CancellationToken cancellationToken = default)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, _tokenEndpoint);
        var parameters = new Dictionary<string, string>
        {
            { "grant_type", "client_credentials" },
            { "client_id", _clientId },
            { "client_secret", _clientSecret },
            { "scope", "repo_analysis" }
        };
        request.Content = new FormUrlEncodedContent(parameters);

        var response = await _httpClient.SendAsync(request, cancellationToken);
        if (!response.IsSuccessStatusCode) throw new Exception("Unable to obtain access token.");
       
        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(content);
        return tokenResponse?.AccessToken;
    }
}