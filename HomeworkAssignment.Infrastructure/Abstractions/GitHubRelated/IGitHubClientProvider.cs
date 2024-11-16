using HomeworkAssignment.Infrastructure.Abstractions.Contracts;

namespace HomeworkAssignment.Infrastructure.Abstractions.GitHubRelated;

public interface IGitHubClientProvider
{
    GitHubClientOptions GetGitHubClientOptions();
}