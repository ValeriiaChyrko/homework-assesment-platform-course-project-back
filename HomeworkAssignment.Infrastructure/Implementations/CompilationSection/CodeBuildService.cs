using System.Diagnostics;
using System.Text;
using HomeworkAssignment.Infrastructure.Abstractions.CompilationSection;

namespace HomeworkAssignment.Infrastructure.Implementations.CompilationSection;

public class CodeBuildService : ICodeBuildService
{
    public async Task<int> BuildProjectAsync(string projectFile, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(projectFile))
            throw new ArgumentException("Project file cannot be null or empty.", nameof(projectFile));

        var extension = Path.GetExtension(projectFile).ToLower();
        var command = GetBuildCommand(extension);
        var arguments = GetBuildArguments(extension, projectFile);

        var processStartInfo = new ProcessStartInfo
        {
            FileName = command,
            Arguments = arguments,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = new Process();
        process.StartInfo = processStartInfo;

        var errorBuilder = new StringBuilder();
        process.ErrorDataReceived += (_, e) =>
        {
            if (!string.IsNullOrEmpty(e.Data)) errorBuilder.AppendLine(e.Data);
        };

        try
        {
            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            await process.WaitForExitAsync(cancellationToken);

            if (process.ExitCode != 0) throw new Exception($"Error: {errorBuilder}");

            return process.ExitCode;
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while processing the project.", ex);
        }
    }

    private static string GetBuildCommand(string extension)
    {
        return extension switch
        {
            ".csproj" => "dotnet",
            ".py" => "python",
            ".java" => "javac",
            _ => throw new NotSupportedException($"Unsupported file type: {extension}")
        };
    }

    private static string GetBuildArguments(string extension, string projectFile)
    {
        return extension switch
        {
            ".csproj" => $"build \"{projectFile}\"",
            ".py" => $"\"{projectFile}\"",
            ".java" => $"{projectFile}",
            _ => throw new NotSupportedException($"Unsupported file type: {extension}")
        };
    }
}