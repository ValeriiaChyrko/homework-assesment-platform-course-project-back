namespace HomeworkAssignment.Infrastructure.Abstractions.CompilationSection;

public interface ICodeBuilder
{
    Task<bool> BuildProjectAsync(string repositoryPath, CancellationToken cancellationToken);
}