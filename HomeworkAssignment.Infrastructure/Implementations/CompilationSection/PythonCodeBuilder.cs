using HomeAssignment.Domain.Abstractions.Contracts;
using HomeworkAssignment.Infrastructure.Abstractions.CompilationSection;
using HomeworkAssignment.Infrastructure.Abstractions.Contracts;
using HomeworkAssignment.Infrastructure.Abstractions.DockerRelated;

namespace HomeworkAssignment.Infrastructure.Implementations.CompilationSection;

public class PythonCodeBuilder : ICodeBuilder
{
    private const string DockerImage = "python:3.11";
    private const string Command = "python3";
    private readonly IDockerService _dockerService;
    private readonly ILogger _logger;

    public PythonCodeBuilder(ILogger logger, IDockerService dockerService)
    {
        _logger = logger;
        _dockerService = dockerService;
    }

    public async Task<bool> BuildProjectAsync(string repositoryPath, CancellationToken cancellationToken)
    {
        var pythonFiles = GetPythonFiles(repositoryPath);
        if (!pythonFiles.Any())
        {
            _logger.Log($"No Python files found in {repositoryPath}.");
            return false;
        }

        var overallSuccess = true;

        foreach (var pythonFile in pythonFiles)
            try
            {
                var result = await BuildPythonFileInDockerAsync(pythonFile, repositoryPath, cancellationToken);

                if (result.ExitCode == 0) continue;

                _logger.Log($"Build failed for {pythonFile} with message: {result.ErrorDataReceived}.");
                overallSuccess = false;
            }
            catch (Exception ex)
            {
                _logger.Log($"Error building Python file {pythonFile}: {ex.Message}");
                overallSuccess = false;
            }

        return overallSuccess;
    }

    private async Task<ProcessResult> BuildPythonFileInDockerAsync(
        string pythonFile,
        string repositoryPath,
        CancellationToken cancellationToken)
    {
        var relativePath = Path.GetRelativePath(repositoryPath, pythonFile);
        var arguments = $"-m py_compile {Path.GetFileName(relativePath)}";
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

    private static string[] GetPythonFiles(string repositoryPath)
    {
        return Directory.GetFiles(repositoryPath, "*.py", SearchOption.AllDirectories);
    }
}