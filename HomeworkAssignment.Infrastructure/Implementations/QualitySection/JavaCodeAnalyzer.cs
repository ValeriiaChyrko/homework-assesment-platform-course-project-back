using System.Collections.Concurrent;
using System.Diagnostics;
using HomeAssignment.Domain.Abstractions.Contracts;
using HomeworkAssignment.Infrastructure.Abstractions.Contracts;
using HomeworkAssignment.Infrastructure.Abstractions.QualitySection;

namespace HomeworkAssignment.Infrastructure.Implementations.QualitySection;

public class JavaCodeAnalyzer : ICodeAnalyzer
{
    private readonly ILogger _logger;

    public JavaCodeAnalyzer(ILogger logger)
    {
        _logger = logger;
    }

    public async Task<IEnumerable<DiagnosticMessage>> AnalyzeAsync(string repositoryPath,
        CancellationToken cancellationToken = default)
    {
        var projectDirectory = Path.Combine(repositoryPath, "src");
        if (!Directory.Exists(projectDirectory))
            throw new DirectoryNotFoundException($"Source directory not found: {projectDirectory}");

        var javaFiles = Directory.GetFiles(projectDirectory, "*.java", SearchOption.AllDirectories);
        var diagnosticsList = new ConcurrentBag<DiagnosticMessage>();

        var tasks = javaFiles.Select(async javaFile =>
        {
            try
            {
                var diagnostics = await AnalyzeJavaFileAsync(javaFile, cancellationToken);
                foreach (var diagnostic in diagnostics) diagnosticsList.Add(diagnostic);
            }
            catch (Exception ex)
            {
                _logger.Log($"Error processing file {javaFile}: {ex.Message}");
            }
        });

        await Task.WhenAll(tasks);
        return diagnosticsList;
    }

    private async Task<IEnumerable<DiagnosticMessage>> AnalyzeJavaFileAsync(string javaFile,
        CancellationToken cancellationToken)
    {
        var diagnostics = new List<DiagnosticMessage>();

        var processStartInfo = new ProcessStartInfo
        {
            FileName = "javac",
            Arguments = $"-Xlint {javaFile}",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = new Process();
        process.StartInfo = processStartInfo;
        process.Start();

        using (var reader = process.StandardError)
        {
            var output = await reader.ReadToEndAsync(cancellationToken);
            if (!string.IsNullOrEmpty(output))
            {
                var lines = output.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                diagnostics.AddRange(from line in lines
                    let severity = DetermineSeverity(line)
                    where !string.IsNullOrEmpty(severity)
                    select new DiagnosticMessage { Message = line, Severity = severity });
            }
        }

        await process.WaitForExitAsync(cancellationToken);

        return diagnostics.Distinct();
    }

    private static string? DetermineSeverity(string message)
    {
        if (message.Contains("error:", StringComparison.OrdinalIgnoreCase))
            return DiagnosticSeverity.Error.ToString();
        if (message.Contains("warning:", StringComparison.OrdinalIgnoreCase))
            return DiagnosticSeverity.Warning.ToString();
        if (message.Contains("info:", StringComparison.OrdinalIgnoreCase))
            return DiagnosticSeverity.Info.ToString();
        return null;
    }
}