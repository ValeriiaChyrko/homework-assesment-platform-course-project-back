using System.Diagnostics;
using HomeworkAssignment.Infrastructure.Abstractions;

namespace HomeworkAssignment.Infrastructure.Implementations;

public class GitHubBuildService : IGitHubBuildService
{
    public async Task<bool> CheckIfProjectCompiles(string owner, string repo, string branch,
        string lastCommitSha, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(lastCommitSha)) return false;

        var repoDirectory = Path.Combine(Path.GetTempPath(), repo);
        if (Directory.Exists(repoDirectory))
        {
            Directory.Delete(repoDirectory, true);
        }

        var cloneCommand = $"git clone https://github.com/{owner}/{repo}.git {repoDirectory}";
        var cloneProcess = await StartProcessAsync(cloneCommand, cancellationToken: cancellationToken);

        await cloneProcess.WaitForExitAsync(cancellationToken);
        if (cloneProcess.ExitCode != 0) return false;

        try
        {
            var checkoutCommand = $"git checkout {branch}";
            var checkoutProcess = await StartProcessAsync(checkoutCommand, repoDirectory, cancellationToken);
            await checkoutProcess.WaitForExitAsync(cancellationToken);
            if (checkoutProcess.ExitCode != 0) return false;
            
            var checkoutCommitCommand = $"git checkout {lastCommitSha}";
            var checkoutCommitProcess = await StartProcessAsync(checkoutCommitCommand, repoDirectory, cancellationToken);
            await checkoutCommitProcess.WaitForExitAsync(cancellationToken);
            if (checkoutCommitProcess.ExitCode != 0) return false;
            
            var buildCommand = "dotnet build"; 
            var buildProcess = await StartProcessAsync(buildCommand, repoDirectory, cancellationToken);
            await buildProcess.WaitForExitAsync(cancellationToken);

            return buildProcess.ExitCode == 0; 
        }
        finally
        {
            if (Directory.Exists(repoDirectory))
            {
                Directory.Delete(repoDirectory, true);
            }
        }
    }
    
    private static async Task<Process> StartProcessAsync(string command, string? workingDirectory = null, CancellationToken cancellationToken = default)
    {
        var processStartInfo = new ProcessStartInfo
        {
            FileName = "cmd.exe",
            Arguments = $"/C {command}",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
            WorkingDirectory = workingDirectory
        };

        var process = new Process { StartInfo = processStartInfo };
        process.Start();
        
        await process.WaitForExitAsync(cancellationToken); 

        return process;
    }
}