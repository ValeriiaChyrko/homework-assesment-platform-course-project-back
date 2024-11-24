using HomeworkAssignment.Infrastructure.Abstractions.Contracts;

namespace HomeworkAssignment.Infrastructure.Abstractions.DockerRelated;

public interface IDockerService
{
    Task<ProcessResult> RunCommandAsync(string repositoryPath, string workingDirectory, string dockerImage, string command,
        string? arguments, CancellationToken cancellationToken);
}