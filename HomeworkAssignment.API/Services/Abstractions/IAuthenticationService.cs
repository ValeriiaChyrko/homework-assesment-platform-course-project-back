namespace HomeworkAssignment.Services.Abstractions;

public interface IAuthenticationService
{
    Task<string?> GetAccessTokenAsync(CancellationToken cancellationToken = default);
}