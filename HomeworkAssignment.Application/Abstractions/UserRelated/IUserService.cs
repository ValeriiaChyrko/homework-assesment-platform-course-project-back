namespace HomeworkAssignment.Application.Abstractions;

public interface IUserService
{
    Task<bool> CheckIfUserInTeacherRole(Guid userId, CancellationToken cancellationToken = default);
    Task<string> GetUserGitHubUsername(Guid userId, CancellationToken cancellationToken = default);
}