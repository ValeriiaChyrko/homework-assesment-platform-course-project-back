using HomeworkAssignment.Infrastructure.Abstractions.Contracts;

namespace HomeworkAssignment.Infrastructure.Abstractions;

public interface IGitHubClientProvider
{
    GitHubClientOptions GetGitHubClientOptions();
}