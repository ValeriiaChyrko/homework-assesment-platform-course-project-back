using HomeAssignment.Domain.Abstractions.Contracts;
using HomeworkAssignment.Infrastructure.Abstractions.Contracts;
using HomeworkAssignment.Infrastructure.Abstractions.Contracts.Interfaces;
using HomeworkAssignment.Infrastructure.Abstractions.DockerRelated;
using HomeworkAssignment.Infrastructure.Abstractions.QualitySection;

namespace HomeworkAssignment.Infrastructure.Implementations.QualitySection;

public class CodeQualityService : ICodeQualityService
{
    private const int MaxScorePercentage = 100;
    private const int MinScorePercentage = 0;

    private static readonly Dictionary<DiagnosticSeverity, int> SeverityWeights = new()
    {
        { DiagnosticSeverity.Error, 35 },
        { DiagnosticSeverity.Warning, 20 },
        { DiagnosticSeverity.Info, 10 }
    };

    private readonly IDockerService _dockerService;

    private readonly ILanguageDetector _languageDetector;
    private readonly ILogger _logger;

    public CodeQualityService(ILogger logger, ILanguageDetector languageDetector, IDockerService dockerService)
    {
        _logger = logger;
        _languageDetector = languageDetector;
        _dockerService = dockerService;
    }

    public async Task<int> CheckCodeQualityAsync(string repositoryPath,
        CancellationToken cancellationToken = default)
    {
        var language = _languageDetector.DetectMainLanguage(repositoryPath);
        ICodeAnalyzer analyzer = language switch
        {
            "C#" => new DotNetCodeAnalyzer(_logger, _dockerService),
            "Python" => new PythonCodeAnalyzer(_logger, _dockerService),
            "Java" => new JavaCodeAnalyzer(_logger, _dockerService),
            _ => throw new NotSupportedException($"Unsupported file type: {language}")
        };

        var diagnostics = await analyzer.AnalyzeAsync(repositoryPath, cancellationToken);
        return EvaluateCodeQuality(diagnostics);
    }

    private static int EvaluateCodeQuality(IEnumerable<DiagnosticMessage>? diagnostics)
    {
        if (diagnostics == null) return MaxScorePercentage;

        var diagnosticMessages = diagnostics.ToList();
        if (diagnosticMessages.Count == 0) return MaxScorePercentage;

        var errorCount = diagnosticMessages.Count(d => d.Severity == DiagnosticSeverity.Error.ToString());
        var warningCount = diagnosticMessages.Count(d => d.Severity == DiagnosticSeverity.Warning.ToString());
        var infoCount = diagnosticMessages.Count(d => d.Severity == DiagnosticSeverity.Info.ToString());

        var score = MaxScorePercentage - (errorCount * SeverityWeights[DiagnosticSeverity.Error] +
                                          warningCount * SeverityWeights[DiagnosticSeverity.Warning] +
                                          infoCount * SeverityWeights[DiagnosticSeverity.Info]);

        return Math.Clamp(score, MinScorePercentage, MaxScorePercentage);
    }
}