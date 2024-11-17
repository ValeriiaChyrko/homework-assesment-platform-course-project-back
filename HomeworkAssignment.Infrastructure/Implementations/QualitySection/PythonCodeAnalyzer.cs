using HomeworkAssignment.Infrastructure.Abstractions.Contracts;
using HomeworkAssignment.Infrastructure.Abstractions.QualitySection;

namespace HomeworkAssignment.Infrastructure.Implementations.QualitySection;

public class PythonCodeAnalyzer : ICodeAnalyzer
{
    public async Task<IEnumerable<DiagnosticMessage>> AnalyzeAsync(string repositoryPath,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}