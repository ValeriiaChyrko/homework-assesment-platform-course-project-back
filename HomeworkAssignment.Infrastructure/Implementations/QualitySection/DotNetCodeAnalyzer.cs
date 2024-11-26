using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using HomeAssignment.Domain.Abstractions.Contracts;
using HomeworkAssignment.Infrastructure.Abstractions.Contracts;
using HomeworkAssignment.Infrastructure.Abstractions.DockerRelated;
using HomeworkAssignment.Infrastructure.Abstractions.QualitySection;

namespace HomeworkAssignment.Infrastructure.Implementations.QualitySection;

public partial class DotNetCodeAnalyzer : ICodeAnalyzer
{
    private const string DockerImage = "mcr.microsoft.com/dotnet/sdk:7.0";
    private const string Command = "dotnet";
    private readonly ILogger _logger;
    private readonly IDockerService _dockerService;

    public DotNetCodeAnalyzer(ILogger logger, IDockerService dockerService)
    {
        _logger = logger;
        _dockerService = dockerService;
    }

    public async Task<IEnumerable<DiagnosticMessage>> AnalyzeAsync(string repositoryPath,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(repositoryPath))
            throw new ArgumentException("Repository path cannot be null or empty.", nameof(repositoryPath));

        var projectFiles = GetProjectFiles(repositoryPath);
        if (projectFiles.Length == 0)
        {
            _logger.Log("No C# project files found.");
            return Enumerable.Empty<DiagnosticMessage>();
        }

        var diagnosticsList = new ConcurrentBag<DiagnosticMessage>();

        foreach (var projectFile in projectFiles)
        {
            try
            {
                var diagnostics = await AnalyzeProjectInDockerAsync(projectFile, repositoryPath, cancellationToken);
                foreach (var diagnostic in diagnostics)
                {
                    diagnosticsList.Add(diagnostic);
                }
            }
            catch (Exception ex)
            {
                _logger.Log($"Error analyzing project {projectFile}: {ex.Message}");
            }
        }

        return diagnosticsList.Distinct();
    }

    private async Task<IEnumerable<DiagnosticMessage>> AnalyzeProjectInDockerAsync(
        string projectFile, 
        string repositoryPath, 
        CancellationToken cancellationToken)
    {
        var relativePath = Path.GetRelativePath(repositoryPath, projectFile);
        var arguments = $"build {Path.GetFileName(relativePath)} --no-incremental /nologo /v:q";
        var workingDirectory = Path.GetDirectoryName(relativePath) ?? string.Empty;

        var result = await _dockerService.RunCommandAsync(
            repositoryPath,
            workingDirectory,
            DockerImage,
            Command,
            arguments,
            cancellationToken
        );
        
        return ParseDiagnostics(result.OutputDataReceived);
    }

    private static string[] GetProjectFiles(string repositoryPath)
    {
        return Directory.GetFiles(repositoryPath, "*.csproj", SearchOption.AllDirectories);
    }
    
    private static IEnumerable<DiagnosticMessage> ParseDiagnostics(string output)
    {
        var diagnostics = new List<DiagnosticMessage>();
        
        var regex = MessageRegex();

        var lines = output.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        foreach (var line in lines)
        {
            var match = regex.Match(line);
            if (!match.Success) continue;

            var severity = match.Groups["severity"].Value.Equals("error", StringComparison.OrdinalIgnoreCase) 
                ? DiagnosticSeverity.Error 
                : DiagnosticSeverity.Warning;

            var message = $"{match.Groups["code"].Value}: {match.Groups["message"].Value} [{match.Groups["project"].Value}]";

            diagnostics.Add(new DiagnosticMessage
            {
                Message = message,
                Severity = severity.ToString()
            });
        }

        return diagnostics;
    }

    [GeneratedRegex(@"(?<severity>error|warning) (?<code>CS\d*): (?<message>.+?) \[(?<project>.+?)\]", RegexOptions.IgnoreCase, "uk-UA")]
    private static partial Regex MessageRegex();
}