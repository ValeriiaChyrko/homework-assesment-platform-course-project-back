using HomeAssignment.Domain.Abstractions.Contracts;
using HomeworkAssignment.Infrastructure.Abstractions.CompilationSection;
using HomeworkAssignment.Infrastructure.Abstractions.Contracts.Interfaces;
using HomeworkAssignment.Infrastructure.Abstractions.DockerRelated;

namespace HomeworkAssignment.Infrastructure.Implementations.CompilationSection;

public class CodeBuildService : ICodeBuildService
{
    private readonly ILanguageDetector _languageDetector;
    private readonly IDockerService _dockerService;
    private readonly ILogger _logger;

    public CodeBuildService(ILogger logger, ILanguageDetector languageDetector, IDockerService dockerService)
    {
        _logger = logger;
        _languageDetector = languageDetector;
        _dockerService = dockerService;
    }

    public async Task<bool> VerifyProjectCompilation(string repositoryPath,
        CancellationToken cancellationToken = default)
    {
        var language = _languageDetector.DetectMainLanguage(repositoryPath);
        ICodeBuilder builder = language switch
        {
            "C#" => new DotNetCodeBuilder(_logger, _dockerService),
            "Python" => new PythonCodeBuilder(_logger),
            "Java" => new JavaCodeBuilder(_logger),
            _ => throw new NotSupportedException($"Unsupported file type: {language}")
        };

        var result = await builder.BuildProjectAsync(repositoryPath, cancellationToken);
        return result;
    }

}