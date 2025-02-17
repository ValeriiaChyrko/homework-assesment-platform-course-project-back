using System.Text.Json;
using HomeworkAssignment.Services.Abstractions;

namespace HomeworkAssignment.Services;

public class KeycloakTokenService : IKeycloakTokenService
{
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;
    private readonly ILogger<KeycloakTokenService> _logger;

    public KeycloakTokenService(HttpClient httpClient, IConfiguration configuration,
        ILogger<KeycloakTokenService> logger)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<string?> GetAccessTokenAsync()
    {
        var keycloakUrl = _configuration["Keycloak:AccessTokenUrl"];
        var clientId = _configuration["Keycloak:ConfidentialClientId"];
        var clientSecret = _configuration["Keycloak:ConfidentialClientSecret"];
        var scope = _configuration["Keycloak:ConfidentialClientScope"];

        if (string.IsNullOrEmpty(keycloakUrl) || string.IsNullOrEmpty(clientId) ||
            string.IsNullOrEmpty(clientSecret) || string.IsNullOrEmpty(scope))
        {
            _logger.LogError("Keycloak configuration is missing.");
            throw new InvalidOperationException("Keycloak configuration is not set properly.");
        }

        var request = new HttpRequestMessage(HttpMethod.Post, keycloakUrl)
        {
            Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "grant_type", "client_credentials" },
                { "client_id", clientId },
                { "client_secret", clientSecret },
                { "scope", scope }
            })
        };

        var response = await _httpClient.SendAsync(request);
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            _logger.LogError("Failed to retrieve access token. Status Code: {StatusCode}, Response: {Response}",
                response.StatusCode, errorContent);
            throw new InvalidOperationException(errorContent);
        }

        var json = await response.Content.ReadAsStringAsync();
        var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(json);
        if (tokenResponse == null)
        {
            _logger.LogError("Failed to deserialize token response.");
            throw new InvalidOperationException("Failed to deserialize token response.");
        }

        return tokenResponse?.AccessToken;
    }
}