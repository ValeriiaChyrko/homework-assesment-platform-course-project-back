using System.Net.Http.Headers;
using HomeworkAssignment.Infrastructure.Abstractions;
using HomeworkAssignment.Infrastructure.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace HomeworkAssignment.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddSingleton<IGitHubClientProvider, GitHubClientProvider>();

        services.AddHttpClient<IGitHubClient, GitHubClient>((provider, client) =>
        {
            var gitHubClientProvider = provider.GetService<IGitHubClientProvider>();
            var options = gitHubClientProvider!.GetGitHubClientOptions();

            client.BaseAddress = new Uri("https://api.github.com/");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", options.Token);
            client.DefaultRequestHeaders.UserAgent.ParseAdd("HomeworkAssignment API v1/1.0");
        });
    }
}