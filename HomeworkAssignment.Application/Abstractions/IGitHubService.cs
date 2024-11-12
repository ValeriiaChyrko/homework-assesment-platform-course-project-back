namespace HomeworkAssignment.Application.Abstractions;

public interface IGitHubService
{
    Task<IEnumerable<string>?> GetStudentBranches(Guid githubProfileId, Guid assignmentId, CancellationToken cancellationToken = default);
}