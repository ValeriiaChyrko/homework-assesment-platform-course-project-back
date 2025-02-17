using System.Text.Json.Serialization;

namespace HomeworkAssignment.Services.Abstractions;

public class TokenResponse
{
    [JsonPropertyName("access_token")] public string? AccessToken { get; init; }

    [JsonPropertyName("expires_in")] public int ExpiresIn { get; init; }

    [JsonPropertyName("refresh_expires_in")]
    public int RefreshExpiresIn { get; init; }

    [JsonPropertyName("token_type")] public string? TokenType { get; init; }

    [JsonPropertyName("id_token")] public string? IdToken { get; init; }

    [JsonPropertyName("not-before-policy")]
    public int NotBeforePolicy { get; init; }

    [JsonPropertyName("scope")] public string? Scope { get; init; }
}