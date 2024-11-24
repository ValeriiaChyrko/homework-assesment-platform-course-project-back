using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text.Json;
using HomeAssignment.Domain.Abstractions.Contracts;
using HomeworkAssignment.Infrastructure.Abstractions.Contracts;
using HomeworkAssignment.Infrastructure.Abstractions.QualitySection;

namespace HomeworkAssignment.Infrastructure.Implementations.QualitySection
{
    public class PythonCodeAnalyzer : ICodeAnalyzer
    {
        private readonly ILogger _logger;

        public PythonCodeAnalyzer(ILogger logger)
        {
            _logger = logger;
        }

        public async Task<IEnumerable<DiagnosticMessage>> AnalyzeAsync(string repositoryPath,
            CancellationToken cancellationToken = default)
        {
            var diagnosticsList = new ConcurrentBag<DiagnosticMessage>();
            var sourcePath = Path.Combine(repositoryPath, "src");
            var pythonFiles = Directory.GetFiles(sourcePath, "*.py", SearchOption.AllDirectories);

            var tasks = pythonFiles.Select(file => AnalyzeFileInDockerAsync(file, diagnosticsList, cancellationToken));
            await Task.WhenAll(tasks);

            return diagnosticsList.Distinct();
        }

        private async Task AnalyzeFileInDockerAsync(string filePath, ConcurrentBag<DiagnosticMessage> diagnosticsList,
            CancellationToken cancellationToken)
        {
            const string dockerImage = "homework-assignment.api:dev"; 
            
            var dockerCommand = $"docker run --rm -v \"{Path.GetDirectoryName(filePath)?.Replace("\\", "/")}:/app\" {dockerImage} pylint --output-format=json /app/{Path.GetFileName(filePath)}";

            var isWindows = Environment.OSVersion.Platform == PlatformID.Win32NT;
            var shellFileName = isWindows ? "cmd.exe" : "/bin/sh";
            var shellArguments = isWindows ? $"/c {dockerCommand}" : $"-c \"{dockerCommand}\"";

            var processStartInfo = new ProcessStartInfo
            {
                FileName = shellFileName,
                Arguments = shellArguments,
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
                        _logger.Log($"Error deserializing pylint output for file {filePath}: {ex.Message}");
                        diagnosticsList.Add(new DiagnosticMessage
                        {
                            Message = $"Error deserializing pylint output for file {filePath}: {ex.Message}",
                            Severity = DiagnosticSeverity.Error.ToString()
                        });
                    }
                }

                await process.WaitForExitAsync(cancellationToken);
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
            finally
            {
                if (!process.HasExited) process.Kill();
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
}
