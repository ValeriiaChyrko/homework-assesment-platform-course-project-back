using HomeAssignment.Domain.Abstractions.Contracts;
using HomeworkAssignment.Infrastructure.Abstractions.CompilationSection;
using HomeworkAssignment.Infrastructure.Abstractions.Contracts;
using HomeworkAssignment.Infrastructure.Abstractions.DockerRelated;

namespace HomeworkAssignment.Infrastructure.Implementations.CompilationSection;

public class JavaCodeBuilder : ICodeBuilder
{
    private const string DockerImage = "maven:3.8.6-jdk-11";
    private const string Command = "mvn";
    private const string Arguments = "compile -q";
    private readonly IDockerService _dockerService;
    private readonly ILogger _logger;

    public JavaCodeBuilder(ILogger logger, IDockerService dockerService)
    {
        _logger = logger;
        _dockerService = dockerService;
    }

    public async Task<bool> BuildProjectAsync(string repositoryPath, CancellationToken cancellationToken)
    {
        var pomFilePath = Path.Combine(repositoryPath, "pom.xml");
        if (!File.Exists(pomFilePath))
        {
            _logger.Log("No pom.xml file found in the specified repository path.");
            return false;
        }

        try
        {
            var result = await BuildProjectInDockerAsync(repositoryPath, cancellationToken);
            return result.ExitCode == 0;
        }
        catch (Exception ex)
        {
            _logger.Log($"Error building Java project: {ex.Message}");
            return false;
        }
    }

    private async Task<ProcessResult> BuildProjectInDockerAsync(string repositoryPath,
        CancellationToken cancellationToken)
    {
        var result = await _dockerService.RunCommandAsync(
            repositoryPath,
            string.Empty,
            DockerImage,
            Command,
            Arguments,
            cancellationToken
        );

        return result;
    }
}