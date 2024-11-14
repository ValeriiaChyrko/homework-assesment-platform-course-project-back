namespace HomeworkAssignment.Infrastructure.Abstractions;

public interface ICodeQualityService
{
    Task<int> CheckCodeQualityAsync(string repoDirectory, CancellationToken cancellationToken = default);
}