namespace HomeworkAssignment.Application.Abstractions;

public interface IGitHubService
{
    Task<IEnumerable<string>?> GetStudentBranches(Guid githubProfileId, Guid assignmentId,
        CancellationToken cancellationToken = default);

    Task<int> VerifyProjectCompilation(Guid githubProfileId, Guid assignmentId, string branch,
        CancellationToken cancellationToken = default);

    Task<int> VerifyProjectQuality(Guid githubProfileId, Guid assignmentId, string branch,
        CancellationToken cancellationToken = default);

    Task<int> VerifyProjectTests(Guid githubProfileId, Guid assignmentId, string branch,
        CancellationToken cancellationToken = default);
}