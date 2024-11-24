using HomeAssignment.Domain.Abstractions.Contracts;
using HomeworkAssignment.Infrastructure.Abstractions.CompilationSection;
using HomeworkAssignment.Infrastructure.Abstractions.Contracts;
using HomeworkAssignment.Infrastructure.Abstractions.DockerRelated;

namespace HomeworkAssignment.Infrastructure.Implementations.CompilationSection;

public class DotNetCodeBuilder : ICodeBuilder
{
    private const string DockerImage = "mcr.microsoft.com/dotnet/sdk:7.0";
    private const string Command = "dotnet";
    private readonly ILogger _logger;
    private readonly IDockerService _dockerService;

    public DotNetCodeBuilder(ILogger logger, IDockerService dockerService)
    {
        _logger = logger;
        _dockerService = dockerService;
    }

    public async Task<bool> BuildProjectAsync(string repositoryPath, CancellationToken cancellationToken)
    {
        var projectFiles = GetProjectFiles(repositoryPath);
        if (!projectFiles.Any())
        {
            _logger.Log($"No project files found in {repositoryPath}.");
            return false;
        }

        var overallSuccess = true;
        
        foreach (var projectFile in projectFiles)
        {
            try
            {
                var result = await BuildProjectFileInDockerAsync(projectFile, repositoryPath, cancellationToken);

                if (result.ExitCode == 0) continue;
                
                _logger.Log($"Build failed for {projectFile} with message: {result.ErrorDataReceived}.");
                overallSuccess = false;
            }
            catch (Exception ex)
            {
                _logger.Log($"Error building project {projectFile}: {ex.Message}");
                overallSuccess = false;
            }
        }

        return overallSuccess;
    }
    
    private async Task<ProcessResult> BuildProjectFileInDockerAsync(
        string projectFile,
        string repositoryPath,
        CancellationToken cancellationToken)
    {
        var relativePath = Path.GetRelativePath(repositoryPath, projectFile);
        var arguments = $"build {Path.GetFileName(relativePath)} /nologo /v:q";
        var workingDirectory = Path.GetDirectoryName(relativePath) ?? string.Empty;

        var result = await _dockerService.RunCommandAsync(
            repositoryPath,
            workingDirectory,
            DockerImage,
            Command,
            arguments,
            cancellationToken
        );

        return result;
    }
    
    private static string[] GetProjectFiles(string repositoryPath)
    {
        return Directory.GetFiles(repositoryPath, "*.csproj", SearchOption.AllDirectories);
    }
}