using System.Diagnostics;
using HomeworkAssignment.Infrastructure.Abstractions.Contracts;
using HomeworkAssignment.Infrastructure.Abstractions.Contracts.Interfaces;
using HomeworkAssignment.Infrastructure.Abstractions.DockerRelated;

namespace HomeworkAssignment.Infrastructure.Implementations.DockerRelated;

public class DockerService : IDockerService
{
    private readonly IProcessService _processService;

    public DockerService(IProcessService processService)
    {
        _processService = processService;
    }


    public async Task<ProcessResult> RunCommandAsync(string repositoryPath, string workingDirectory, string dockerImage,
        string command, string? arguments, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(repositoryPath))
            throw new ArgumentException("Repository path cannot be null or empty.", nameof(repositoryPath));

        if (string.IsNullOrWhiteSpace(dockerImage))
            throw new ArgumentException("Docker image cannot be null or empty.", nameof(dockerImage));

        if (string.IsNullOrWhiteSpace(command))
            throw new ArgumentException("Command cannot be null or empty.", nameof(command));

        var dockerCommand =
            $"docker run -it --rm -v \"{repositoryPath}:/workspace\" -w /workspace/{workingDirectory} {dockerImage} {command} {arguments}";

        var isWindows = Environment.OSVersion.Platform == PlatformID.Win32NT;
        var shellFileName = isWindows ? "cmd.exe" : "sh";
        var shellArguments = isWindows ? $"/c \"{dockerCommand.Replace("\\", "/")}\"" : $"-c \"{dockerCommand}\"";

        var processStartInfo = new ProcessStartInfo
        {
            FileName = shellFileName,
            Arguments = shellArguments,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        return await _processService.RunProcessAsync(processStartInfo, cancellationToken);
    }
}