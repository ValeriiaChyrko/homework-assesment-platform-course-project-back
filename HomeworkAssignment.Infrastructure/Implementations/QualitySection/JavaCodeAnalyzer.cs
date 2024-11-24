using System.Collections.Concurrent;
using HomeAssignment.Domain.Abstractions.Contracts;
using HomeworkAssignment.Infrastructure.Abstractions.Contracts;
using HomeworkAssignment.Infrastructure.Abstractions.DockerRelated;
using HomeworkAssignment.Infrastructure.Abstractions.QualitySection;

namespace HomeworkAssignment.Infrastructure.Implementations.QualitySection
{
    public class JavaCodeAnalyzer : ICodeAnalyzer
    {
        private const string DockerImage = "maven:3.8.6-jdk-11";
        private const string Command = "mvn";
        private readonly ILogger _logger;
        private readonly IDockerService _dockerService;

        public JavaCodeAnalyzer(ILogger logger, IDockerService dockerService)
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
                _logger.Log($"No Maven project files found in the {repositoryPath}.");
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
            const string arguments = "compile -e";
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
            return Directory.GetFiles(repositoryPath, "pom.xml", SearchOption.AllDirectories);
        }

        private static IEnumerable<DiagnosticMessage> ParseDiagnostics(string output)
        {
            var diagnostics = new List<DiagnosticMessage>();

            var lines = output.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                if (!line.Contains("error") && !line.Contains("warning")) continue;
                var severity = line.Contains("error") ? DiagnosticSeverity.Error : DiagnosticSeverity.Warning;
                diagnostics.Add(new DiagnosticMessage
                {
                    Message = line,
                    Severity = severity.ToString()
                });
            }

            return diagnostics;
        }
    }
}
