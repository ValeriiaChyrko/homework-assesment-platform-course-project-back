namespace HomeworkAssignment.Application.Abstractions.Contracts;

public interface IKeycloakTokenService
{
    Task<string?> GetAccessTokenAsync();
}