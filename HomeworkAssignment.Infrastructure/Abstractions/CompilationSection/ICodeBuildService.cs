namespace HomeworkAssignment.Infrastructure.Abstractions.CompilationSection;

public interface ICodeBuildService
{
    Task<int> BuildProjectAsync(string projectFile, CancellationToken cancellationToken = default);
}