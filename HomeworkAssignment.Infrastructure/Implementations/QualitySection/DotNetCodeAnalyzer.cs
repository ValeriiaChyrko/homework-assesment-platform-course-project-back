using System.Collections.Concurrent;
using HomeAssignment.Domain.Abstractions.Contracts;
using HomeworkAssignment.Infrastructure.Abstractions.Contracts;
using HomeworkAssignment.Infrastructure.Abstractions.QualitySection;
using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;
using DiagnosticSeverity = HomeworkAssignment.Infrastructure.Abstractions.Contracts.DiagnosticSeverity;

namespace HomeworkAssignment.Infrastructure.Implementations.QualitySection;

public class DotNetCodeAnalyzer : ICodeAnalyzer
{
    private readonly ILogger _logger;
    private readonly MSBuildWorkspace _workspace;

    public DotNetCodeAnalyzer(MSBuildWorkspace workspace, ILogger logger)
    {
        _workspace = workspace ?? throw new ArgumentNullException(nameof(workspace));
        _logger = logger;
    }

    public async Task<IEnumerable<DiagnosticMessage>> AnalyzeAsync(string repositoryPath,
        CancellationToken cancellationToken = default)
    {
        if (!MSBuildLocator.IsRegistered) MSBuildLocator.RegisterDefaults();

        var projects = Directory.GetFiles(repositoryPath, "*.csproj", SearchOption.AllDirectories);
        var diagnosticsList = new ConcurrentBag<DiagnosticMessage>();

        var tasks = projects.Select(async projectFile =>
        {
            try
            {
                var project = await _workspace.OpenProjectAsync(projectFile, cancellationToken: cancellationToken);
                var compilation = await project.GetCompilationAsync(cancellationToken);

                if (compilation != null)
                {
                    var diagnostics = compilation.GetDiagnostics(cancellationToken);
                    AddDiagnosticsToList(diagnostics, diagnosticsList);
                }
            }
            catch (Exception ex)
            {
                _logger.Log($"Error processing project {projectFile}: {ex.Message}");
            }
        });

        await Task.WhenAll(tasks);
        return diagnosticsList;
    }

    private static void AddDiagnosticsToList(IEnumerable<Diagnostic> diagnostics,
        ConcurrentBag<DiagnosticMessage>? diagnosticsList)
    {
        foreach (var diagnostic in diagnostics)
        {
            if (!Enum.TryParse<DiagnosticSeverity>(diagnostic.Severity.ToString(), out var severity)) continue;

            var message = diagnostic.ToString();
            diagnosticsList?.Add(new DiagnosticMessage
            {
                Message = message,
                Severity = severity.ToString()
            });
        }
    }
}