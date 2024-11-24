using HomeAssignment.Domain.Abstractions.Contracts;
using HomeworkAssignment.Infrastructure.Abstractions.Contracts;
using HomeworkAssignment.Infrastructure.Abstractions.Contracts.Interfaces;
using HomeworkAssignment.Infrastructure.Abstractions.DockerRelated;
using HomeworkAssignment.Infrastructure.Abstractions.TestsSection;

namespace HomeworkAssignment.Infrastructure.Implementations.TestsSection;

public class CodeTestsService : ICodeTestsService
{
    private const int MaxScorePercentage = 100;
    private const int MinScorePercentage = 0;
    
    private readonly ILanguageDetector _languageDetector;
    private readonly IDockerService _dockerService;
    private readonly ILogger _logger;

    public CodeTestsService(ILogger logger, ILanguageDetector languageDetector, IDockerService dockerService)
    {
        _logger = logger;
        _languageDetector = languageDetector;
        _dockerService = dockerService;
    }

    public async Task<int> CheckCodeTestsAsync(string repositoryDirectory,
        CancellationToken cancellationToken = default)
    {
        var language = _languageDetector.DetectMainLanguage(repositoryDirectory);
        ITestsRunner runner = language switch
        {
            "C#" => new DotNetTestsRunner(_logger, _dockerService),
            "Python" => new PythonTestsRunner(_logger, _dockerService),
            "Java" => new JavaTestsRunner(_logger, _dockerService),
            _ => throw new NotSupportedException($"Unsupported file type: {language}")
        };

        var results = await runner.RunTestsAsync(repositoryDirectory, cancellationToken);
        return EvaluateTestResults(results);
    }

    private static int EvaluateTestResults(IEnumerable<TestResult>? testResults)
    {
        if (testResults == null) return MaxScorePercentage;

        var results = testResults.ToList();
        if (results.Count == 0) return MaxScorePercentage;

        var passed = results.Count(tr => tr.IsPassed);
        var failed = results.Count(tr => !tr.IsPassed);

        var score = (int)(passed / (double)(passed + failed) * MaxScorePercentage);
        return Math.Clamp(score, MinScorePercentage, MaxScorePercentage);
    }
}