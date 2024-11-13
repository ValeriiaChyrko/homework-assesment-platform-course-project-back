using System.Diagnostics;
using HomeworkAssignment.Infrastructure.Abstractions;

namespace HomeworkAssignment.Infrastructure.Implementations;

public class BuildService : IBuildService
{
    public async Task<bool> BuildProject(string projectFile)
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

        var error = await process.StandardError.ReadToEndAsync();
        await process.WaitForExitAsync();

        if (process.ExitCode != 0)
        {
            throw new Exception($"Build failed for project '{projectFile}'. Error: {error}");
        }

        return true; // Build succeeded
    }
}