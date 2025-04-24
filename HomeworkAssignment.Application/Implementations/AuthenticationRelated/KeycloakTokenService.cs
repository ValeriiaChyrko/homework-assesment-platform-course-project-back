using System.Text.Json;
using HomeworkAssignment.Application.Abstractions.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace HomeworkAssignment.Application.Implementations.AuthenticationRelated;

public class KeycloakTokenService(
    HttpClient httpClient,
    IConfiguration configuration,
    ILogger<KeycloakTokenService> logger)
    : IKeycloakTokenService
{
    public async Task<string?> GetAccessTokenAsync()
    {
        var keycloakUrl = configuration["Keycloak:AccessTokenUrl"];
        var clientId = configuration["Keycloak:ConfidentialClientId"];
        var clientSecret = configuration["Keycloak:ConfidentialClientSecret"];
        var scope = configuration["Keycloak:ConfidentialClientScope"];

        if (string.IsNullOrEmpty(keycloakUrl) || string.IsNullOrEmpty(clientId) ||
            string.IsNullOrEmpty(clientSecret) || string.IsNullOrEmpty(scope))
        {
            logger.LogError("Missing Keycloak configuration.");
            throw new InvalidOperationException("Keycloak configuration is not set properly.");
        }

        logger.LogInformation("Requesting Keycloak access token.");

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

        var response = await httpClient.SendAsync(request);
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            logger.LogError("Failed to retrieve Keycloak token. Status: {StatusCode}, Body: {Response}",
                response.StatusCode, errorContent);
            throw new InvalidOperationException(errorContent);
        }

        var json = await response.Content.ReadAsStringAsync();
        var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(json);
        if (tokenResponse == null)
        {
            logger.LogError("Failed to parse Keycloak token response.");
            throw new InvalidOperationException("Failed to deserialize token response.");
        }

        logger.LogInformation("Successfully retrieved Keycloak access token.");
        return tokenResponse.AccessToken;
    }
}