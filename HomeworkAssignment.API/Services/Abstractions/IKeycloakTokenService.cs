namespace HomeworkAssignment.Services.Abstractions;

public interface IKeycloakTokenService
{
    Task<string?> GetAccessTokenAsync();
}