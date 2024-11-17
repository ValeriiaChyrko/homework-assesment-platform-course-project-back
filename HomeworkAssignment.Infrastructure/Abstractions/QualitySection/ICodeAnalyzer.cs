using HomeworkAssignment.Infrastructure.Abstractions.Contracts;

namespace HomeworkAssignment.Infrastructure.Abstractions.QualitySection;

public interface ICodeAnalyzer
{
    Task<IEnumerable<DiagnosticMessage>>
        AnalyzeAsync(string repositoryPath, CancellationToken cancellationToken = default);
}