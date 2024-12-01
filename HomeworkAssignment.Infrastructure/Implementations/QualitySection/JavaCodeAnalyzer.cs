using HomeAssignment.Domain.Abstractions.Contracts;
using HomeworkAssignment.Infrastructure.Abstractions.Contracts;
using HomeworkAssignment.Infrastructure.Abstractions.DockerRelated;
using HomeworkAssignment.Infrastructure.Abstractions.QualitySection;

namespace HomeworkAssignment.Infrastructure.Implementations.QualitySection;

public class JavaCodeAnalyzer : ICodeAnalyzer
{
    private const string DockerImage = "maven:3.8.6-jdk-11";
    private const string Command = "mvn";
    private const string CompileArguments = "compile -e";
    private readonly IDockerService _dockerService;

    private readonly ILogger _logger;

    public JavaCodeAnalyzer(ILogger logger, IDockerService dockerService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _dockerService = dockerService ?? throw new ArgumentNullException(nameof(dockerService));
    }

    public async Task<IEnumerable<DiagnosticMessage>> AnalyzeAsync(
        string repositoryPath,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(repositoryPath))
            throw new ArgumentException("Repository path cannot be null or empty.", nameof(repositoryPath));

        try
        {
            _logger.Log($"Starting analysis for repository: {repositoryPath}");

            var diagnostics = await AnalyzeProjectInDockerAsync(repositoryPath, cancellationToken);

            _logger.Log($"Analysis completed for repository: {repositoryPath}");
            return diagnostics.Distinct();
        }
        catch (Exception ex)
        {
            _logger.Log($"Error analyzing project at '{repositoryPath}': {ex.Message}");
            return Enumerable.Empty<DiagnosticMessage>();
        }
    }

    private async Task<IEnumerable<DiagnosticMessage>> AnalyzeProjectInDockerAsync(
        string repositoryPath,
        CancellationToken cancellationToken)
    {
        var result = await _dockerService.RunCommandAsync(
            repositoryPath,
            string.Empty,
            DockerImage,
            Command,
            CompileArguments,
            cancellationToken
        );

        return ParseDiagnostics(result.OutputDataReceived);
    }

    private static IEnumerable<DiagnosticMessage> ParseDiagnostics(string output)
    {
        if (string.IsNullOrWhiteSpace(output)) return Enumerable.Empty<DiagnosticMessage>();

        var diagnostics = new List<DiagnosticMessage>();
        var lines = output.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

        foreach (var line in lines)
        {
            var severity = GetSeverityFromLine(line);
            if (severity == null) continue;

            diagnostics.Add(new DiagnosticMessage
            {
                Message = line.Trim(),
                Severity = severity.Value.ToString()
            });
        }

        return diagnostics;
    }

    private static DiagnosticSeverity? GetSeverityFromLine(string line)
    {
        if (line.Contains("error", StringComparison.OrdinalIgnoreCase)) return DiagnosticSeverity.Error;

        if (line.Contains("warning", StringComparison.OrdinalIgnoreCase)) return DiagnosticSeverity.Warning;

        return null;
    }
}