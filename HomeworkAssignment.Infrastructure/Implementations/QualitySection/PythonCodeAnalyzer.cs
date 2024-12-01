using System.Collections.Concurrent;
using System.Text.Json;
using HomeAssignment.Domain.Abstractions.Contracts;
using HomeworkAssignment.Infrastructure.Abstractions.Contracts;
using HomeworkAssignment.Infrastructure.Abstractions.DockerRelated;
using HomeworkAssignment.Infrastructure.Abstractions.QualitySection;

namespace HomeworkAssignment.Infrastructure.Implementations.QualitySection;

public class PythonCodeAnalyzer : ICodeAnalyzer
{
    private const string DockerImage = "python:3.11";
    private const string Command = "pylint";
    private readonly IDockerService _dockerService;
    private readonly ILogger _logger;

    public PythonCodeAnalyzer(ILogger logger, IDockerService dockerService)
    {
        _logger = logger;
        _dockerService = dockerService;
    }

    public async Task<IEnumerable<DiagnosticMessage>> AnalyzeAsync(string repositoryPath,
        CancellationToken cancellationToken = default)
    {
        var diagnosticsList = new ConcurrentBag<DiagnosticMessage>();
        var sourcePath = Path.Combine(repositoryPath, "src");
        var pythonFiles = Directory.GetFiles(sourcePath, "*.py", SearchOption.AllDirectories);

        var tasks = pythonFiles.Select(
            file => AnalyzeFileInDockerAsync(file, repositoryPath, diagnosticsList, cancellationToken)
        );
        await Task.WhenAll(tasks);

        return diagnosticsList.Distinct();
    }

    private async Task AnalyzeFileInDockerAsync(string filePath, string repositoryPath,
        ConcurrentBag<DiagnosticMessage> diagnosticsList, CancellationToken cancellationToken)
    {
        var fileDirectory = Path.GetDirectoryName(filePath);
        if (fileDirectory == null) return;

        var arguments = $"--output-format=json {Path.GetFileName(filePath)}";
        var workingDirectory = Path.GetFileName(fileDirectory);

        try
        {
            var result = await _dockerService.RunCommandAsync(
                repositoryPath,
                workingDirectory,
                DockerImage,
                Command,
                arguments,
                cancellationToken
            );

            if (!string.IsNullOrEmpty(result.OutputDataReceived))
                ProcessPylintOutput(result.OutputDataReceived, filePath, diagnosticsList);
        }
        catch (Exception ex)
        {
            _logger.Log($"Error analyzing file {filePath}: {ex.Message}");
            diagnosticsList.Add(new DiagnosticMessage
            {
                Message = $"Error analyzing file {filePath}: {ex.Message}",
                Severity = DiagnosticSeverity.Error.ToString()
            });
        }
    }

    private void ProcessPylintOutput(string output, string filePath, ConcurrentBag<DiagnosticMessage> diagnosticsList)
    {
        try
        {
            var pylintDiagnostics = JsonSerializer.Deserialize<List<PylintMessage>>(output);
            if (pylintDiagnostics == null) return;

            var filteredDiagnostics = pylintDiagnostics
                .Where(d => !string.IsNullOrEmpty(d.Message) && !string.IsNullOrEmpty(d.Type))
                .ToList();

            foreach (var diagnostic in filteredDiagnostics)
            {
                var severity = DetermineSeverity(diagnostic.Type);
                if (string.IsNullOrEmpty(severity)) continue;

                diagnosticsList.Add(new DiagnosticMessage
                {
                    Message = diagnostic.Message,
                    Severity = severity
                });
            }
        }
        catch (JsonException ex)
        {
            _logger.Log($"Error deserializing pylint output for file {filePath}: {ex.Message}");
            diagnosticsList.Add(new DiagnosticMessage
            {
                Message = $"Error deserializing pylint output for file {filePath}: {ex.Message}",
                Severity = DiagnosticSeverity.Error.ToString()
            });
        }
    }

    private static string? DetermineSeverity(string type)
    {
        return type.ToLower() switch
        {
            "error" => DiagnosticSeverity.Error.ToString(),
            "warning" => DiagnosticSeverity.Warning.ToString(),
            "info" => DiagnosticSeverity.Info.ToString(),
            _ => null
        };
    }
}