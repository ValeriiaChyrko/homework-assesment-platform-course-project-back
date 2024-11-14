namespace HomeworkAssignment.Infrastructure.Abstractions;

public interface ICodeBuildService
{
    Task<int> BuildProjectAsync(string projectFile, CancellationToken cancellationToken = default);
}