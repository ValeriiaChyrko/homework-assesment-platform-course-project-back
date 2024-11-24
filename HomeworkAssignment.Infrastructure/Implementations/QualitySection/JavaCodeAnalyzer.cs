using System.Diagnostics;
using System.Text;
using HomeAssignment.Domain.Abstractions.Contracts;
using HomeworkAssignment.Infrastructure.Abstractions.Contracts;
using HomeworkAssignment.Infrastructure.Abstractions.QualitySection;

namespace HomeworkAssignment.Infrastructure.Implementations.QualitySection
{
    public class JavaCodeAnalyzer : ICodeAnalyzer
    {
        private readonly ILogger _logger;

        public JavaCodeAnalyzer(ILogger logger)
        {
            _logger = logger;
        }

        public async Task<IEnumerable<DiagnosticMessage>> AnalyzeAsync(string repositoryPath, CancellationToken cancellationToken = default)
        {
            if (!Directory.Exists(repositoryPath))
                throw new DirectoryNotFoundException($"Source directory not found: {repositoryPath}");

            return await AnalyzeJavaCodeInDockerAsync(repositoryPath, cancellationToken);
        }

        private async Task<IEnumerable<DiagnosticMessage>> AnalyzeJavaCodeInDockerAsync(string projectDirectory, CancellationToken cancellationToken)
        {
            const string dockerImage = "maven"; 
            var dockerCommand = $"docker run -v \"{projectDirectory.Replace("\\", "/")}:/app\" --rm -w /app {dockerImage} mvn compile -e";

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

            var outputBuilder = new StringBuilder();
            var errorBuilder = new StringBuilder();

            process.OutputDataReceived += (_, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                    outputBuilder.AppendLine(e.Data);
            };

            process.ErrorDataReceived += (_, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                    errorBuilder.AppendLine(e.Data);
            };

            try
            {
                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                await process.WaitForExitAsync(cancellationToken);

                var output = outputBuilder.ToString();
                var error = errorBuilder.ToString();

                if (process.ExitCode == 0)
                {
                    _logger.Log($"Docker analysis succeeded. Output:\n{output}");
                    return ParseDiagnostics(output);
                }

                _logger.Log($"Docker analysis failed.\nOutput:\n{output}\nError:\n{error}");
                throw new Exception($"Docker analysis failed. Output:\n{output}\nError:\n{error}");
            }
            catch (Exception ex)
            {
                _logger.Log($"An error occurred while analyzing Java code in Docker with message: {ex.Message}.");
                throw new Exception("An error occurred while analyzing Java code in Docker.", ex);
            }
            finally
            {
                if (!process.HasExited) process.Kill();
            }
        }

        private static IEnumerable<DiagnosticMessage> ParseDiagnostics(string output)
        {
            var diagnostics = new List<DiagnosticMessage>();
            var lines = output.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in lines)
            {
                var severity = DetermineSeverity(line);
                if (!string.IsNullOrEmpty(severity))
                {
                    diagnostics.Add(new DiagnosticMessage { Message = line, Severity = severity });
                }
            }

            return diagnostics.Distinct();
        }

        private static string DetermineSeverity(string message)
        {
            if (message.Contains("error:", StringComparison.OrdinalIgnoreCase))
                return DiagnosticSeverity.Error.ToString();
            if (message.Contains("warning:", StringComparison.OrdinalIgnoreCase))
                return DiagnosticSeverity.Warning.ToString();
            return DiagnosticSeverity.Info.ToString();
        }
    }
}
