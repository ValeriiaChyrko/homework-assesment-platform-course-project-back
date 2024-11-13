namespace HomeworkAssignment.Application.Abstractions;

public interface IGitHubService
{
    Task<IEnumerable<string>?> GetStudentBranches(Guid githubProfileId, Guid assignmentId, CancellationToken cancellationToken = default);
    Task<bool> VerifyProjectCompilation(Guid githubProfileId, Guid assignmentId, string branch, CancellationToken cancellationToken = default);
}