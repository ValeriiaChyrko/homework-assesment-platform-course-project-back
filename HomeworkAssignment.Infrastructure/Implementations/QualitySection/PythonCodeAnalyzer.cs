using System.Diagnostics;
using System.Text.Json;
using HomeworkAssignment.Infrastructure.Abstractions.Contracts;
using HomeworkAssignment.Infrastructure.Abstractions.QualitySection;
using System.Collections.Concurrent;

namespace HomeworkAssignment.Infrastructure.Implementations.QualitySection;

public class PythonCodeAnalyzer : ICodeAnalyzer
{
    public async Task<IEnumerable<DiagnosticMessage>> AnalyzeAsync(string repositoryPath, CancellationToken cancellationToken = default)
    {
        var diagnosticsList = new ConcurrentBag<DiagnosticMessage>();
        var sourcePath = Path.Combine(repositoryPath, "src");
        var pythonFiles = Directory.GetFiles(sourcePath, "*.py", SearchOption.AllDirectories);

        var tasks = pythonFiles.Select(file => AnalyzeFileAsync(file, diagnosticsList, cancellationToken));
        await Task.WhenAll(tasks);

        return diagnosticsList.Distinct();
    }

private static async Task AnalyzeFileAsync(string filePath, ConcurrentBag<DiagnosticMessage> diagnosticsList, CancellationToken cancellationToken)
{
    var processStartInfo = new ProcessStartInfo
    {
        FileName = "pylint",
        Arguments = $"\"{filePath}\" --output-format=json",
        RedirectStandardOutput = true,
        RedirectStandardError = true,
        UseShellExecute = false,
        CreateNoWindow = true
    };

    using var process = new Process();
    process.StartInfo = processStartInfo;

    try
    {
        process.Start();

        using var reader = process.StandardOutput;
        var output = await reader.ReadToEndAsync(cancellationToken);

        if (!string.IsNullOrEmpty(output))
        {
            try
            {
                var pylintDiagnostics = JsonSerializer.Deserialize<List<PylintMessage>>(output);
                if (pylintDiagnostics != null)
                {
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
            }
            catch (JsonException ex)
            {
                diagnosticsList.Add(new DiagnosticMessage
                {
                    Message = $"Error deserializing pylint output for file {filePath}: {ex.Message}",
                    Severity = DiagnosticSeverity.Error.ToString()
                });
            }
        }

        await process.WaitForExitAsync(cancellationToken);
    }
    catch (Exception ex) when (!(ex is OperationCanceledException))
    {
        diagnosticsList.Add(new DiagnosticMessage
        {
            Message = $"Error analyzing file {filePath}: {ex.Message}",
            Severity = DiagnosticSeverity.Error.ToString()
        });
    }
    finally
    {
        if (!process.HasExited)
        {
            process.Kill();
        }
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
