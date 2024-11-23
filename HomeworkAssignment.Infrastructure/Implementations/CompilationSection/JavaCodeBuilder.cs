using System.Diagnostics;
using System.Text;
using HomeAssignment.Domain.Abstractions.Contracts;
using HomeworkAssignment.Infrastructure.Abstractions.CompilationSection;

namespace HomeworkAssignment.Infrastructure.Implementations.CompilationSection;

public class JavaCodeBuilder : ICodeBuilder
{
    private readonly ILogger _logger;

    public JavaCodeBuilder(ILogger logger)
    {
        _logger = logger;
    }

    public async Task<bool> BuildProjectAsync(string repositoryPath, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(repositoryPath))
            throw new ArgumentException("Repository path cannot be null or empty.", nameof(repositoryPath));
        
        var pomFilePath = Path.Combine(repositoryPath, "pom.xml");
        if (!File.Exists(pomFilePath))
        {
            _logger.Log("No pom.xml file found in the specified repository path.");
            return false;
        }

        try
        {
            var exitCode = await BuildProjectInDockerAsync(repositoryPath, cancellationToken);
            return exitCode == 0;
        }
        catch (Exception ex)
        {
            _logger.Log($"Error building Java project: {ex.Message}");
            return false;
        }
    }

    private async Task<int> BuildProjectInDockerAsync(string repositoryPath, CancellationToken cancellationToken)
    {
        const string dockerImage = "maven:3.8.6-jdk-11";
        const string command = "mvn";
        const string arguments = "compile";

        var dockerCommand = $"docker run -it --rm -v \"{repositoryPath.Replace("\\", "/")}:/workspace\" -w /workspace {dockerImage} {command} {arguments}";

        var isWindows = Environment.OSVersion.Platform == PlatformID.Win32NT;
        var shellFileName = isWindows ? "cmd.exe" : "sh";
        var shellArguments = isWindows ? $"/c \"{dockerCommand}\"" : $"-c \"{dockerCommand}\"";

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
            if (!string.IsNullOrEmpty(e.Data)) outputBuilder.AppendLine(e.Data);
        };
        process.ErrorDataReceived += (_, e) =>
        {
            if (!string.IsNullOrEmpty(e.Data)) errorBuilder.AppendLine(e.Data);
        };

        try
        {
            process.Start();
            process.BeginOutputReadLine();

            await process.WaitForExitAsync(cancellationToken);

            var output = outputBuilder.ToString();
            var error = errorBuilder.ToString();

            if (process.ExitCode == 0) return process.ExitCode;
            
            _logger.Log($"Docker build failed.\nOutput:\n{output}\nError:\n{error}\n");
            throw new Exception($"Docker build failed.\nOutput:\n{output}\nError:\n{error}\n");
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while building the Java project in Docker.", ex);
        }
    }
}