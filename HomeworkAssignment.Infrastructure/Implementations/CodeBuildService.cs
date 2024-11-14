using System.Diagnostics;
using HomeworkAssignment.Infrastructure.Abstractions;

namespace HomeworkAssignment.Infrastructure.Implementations;

public class CodeBuildService : ICodeBuildService
{
    public async Task<int> BuildProjectAsync(string projectFile, CancellationToken cancellationToken = default)
    {
        var processStartInfo = new ProcessStartInfo
        {
            FileName = "dotnet",
            Arguments = $"build \"{projectFile}\"",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = new Process();
        process.StartInfo = processStartInfo;
        process.Start();

        var error = await process.StandardError.ReadToEndAsync(cancellationToken);
        await process.WaitForExitAsync(cancellationToken);

        if (process.ExitCode != 0)
        {
            throw new Exception($"Build failed for project '{projectFile}'. Error: {error}");
        }

        return process.ExitCode; 
    }
}