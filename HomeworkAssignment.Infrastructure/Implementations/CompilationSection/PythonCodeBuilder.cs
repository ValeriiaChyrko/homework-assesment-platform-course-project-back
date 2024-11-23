using System.Diagnostics;
using System.Text;
using HomeAssignment.Domain.Abstractions.Contracts;
using HomeworkAssignment.Infrastructure.Abstractions.CompilationSection;

namespace HomeworkAssignment.Infrastructure.Implementations.CompilationSection;

public class PythonCodeBuilder : ICodeBuilder
{
    private readonly ILogger _logger;

    public PythonCodeBuilder(ILogger logger)
    {
        _logger = logger;
    }

    public async Task<bool> BuildProjectAsync(string repositoryPath, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(repositoryPath))
            throw new ArgumentException("Repository path cannot be null or empty.", nameof(repositoryPath));

        var pythonFiles = GetPythonFiles(repositoryPath);
        if (pythonFiles.Length == 0)
        {
            _logger.Log("No Python files found.");
            return false;
        }

        var overallSuccess = true;

        foreach (var pythonFile in pythonFiles)
        {
            try
            {
                var exitCode = await BuildProjectInDockerAsync(pythonFile, repositoryPath, cancellationToken);
                if (exitCode != 0)
                {
                    overallSuccess = false;
                }
            }
            catch (Exception ex)
            {
                _logger.Log($"Error building Python file {pythonFile}: {ex.Message}");
                overallSuccess = false;
            }
        }

        return overallSuccess;
    }

    private static string[] GetPythonFiles(string repositoryPath)
    {
        return Directory.GetFiles(repositoryPath, "*.py", SearchOption.AllDirectories);
    }

    private async Task<int> BuildProjectInDockerAsync(string projectFile, string repositoryPath, CancellationToken cancellationToken)
    {
        const string dockerImage = "python:3.11";
        const string command = "python3";

        var relativeProjectPath = Path.GetRelativePath(repositoryPath, projectFile).Replace("\\", "/");
        var fileName = Path.GetFileName(relativeProjectPath);
        var arguments = $"-m py_compile {fileName}";
        var workingDirectory = Path.GetDirectoryName(relativeProjectPath)?.Replace("\\", "/");

        var dockerCommand = $"docker run -it --rm -v \"{repositoryPath.Replace("\\", "/")}:/workspace\" -w /workspace/{workingDirectory} {dockerImage} {command} {arguments}";
        
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
            throw new Exception("An error occurred while building the Python project in Docker.", ex);
        }
    }
}