namespace HomeworkAssignment.Infrastructure.Abstractions.TestsSection;

public interface ICodeTestsService
{
    Task<int> CheckCodeTestsAsync(string repositoryDirectory, CancellationToken cancellationToken = default);
}