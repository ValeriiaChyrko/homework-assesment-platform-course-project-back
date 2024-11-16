namespace HomeworkAssignment.Infrastructure.Abstractions.QualitySection;

public interface ICodeQualityService
{
    Task<int> CheckCodeQualityAsync(string repositoryPath, CancellationToken cancellationToken = default);
}