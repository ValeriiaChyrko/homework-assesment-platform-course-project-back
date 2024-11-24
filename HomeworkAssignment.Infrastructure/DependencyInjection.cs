using System.Net.Http.Headers;
using HomeworkAssignment.Infrastructure.Abstractions.CompilationSection;
using HomeworkAssignment.Infrastructure.Abstractions.Contracts.Interfaces;
using HomeworkAssignment.Infrastructure.Abstractions.DockerRelated;
using HomeworkAssignment.Infrastructure.Abstractions.GitHubRelated;
using HomeworkAssignment.Infrastructure.Abstractions.GitRelated;
using HomeworkAssignment.Infrastructure.Abstractions.QualitySection;
using HomeworkAssignment.Infrastructure.Abstractions.TestsSection;
using HomeworkAssignment.Infrastructure.Implementations.CompilationSection;
using HomeworkAssignment.Infrastructure.Implementations.DockerRelated;
using HomeworkAssignment.Infrastructure.Implementations.GitHubRelated;
using HomeworkAssignment.Infrastructure.Implementations.GitRelated;
using HomeworkAssignment.Infrastructure.Implementations.Helpers;
using HomeworkAssignment.Infrastructure.Implementations.QualitySection;
using HomeworkAssignment.Infrastructure.Implementations.TestsSection;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.Extensions.DependencyInjection;

namespace HomeworkAssignment.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddSingleton<IGitHubClientProvider, GitHubClientProvider>();
        services.AddSingleton<MSBuildWorkspace>(_ => MSBuildWorkspace.Create(new Dictionary<string, string>
        {
            { "AlwaysCompileMarkupFilesInSeparateDomain", "true" },
            { "Configuration", "Debug" },
            { "Platform", "AnyCPU" }
        }));

        services.AddHttpClient<IGitHubApiClient, GitHubApiApiClient>((provider, client) =>
        {
            var gitHubClientProvider = provider.GetService<IGitHubClientProvider>();
            var options = gitHubClientProvider!.GetGitHubClientOptions();

            client.BaseAddress = new Uri("https://api.github.com/");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", options.Token);
            client.DefaultRequestHeaders.UserAgent.ParseAdd("HomeworkAssignment API v1/1.0");
        });

        services.AddScoped<IGitService, GitService>();
        services.AddScoped<IBranchService, BranchService>();
        services.AddScoped<ICommitService, CommitService>();

        services.AddScoped<ICodeBuildService, CodeBuildService>();
        services.AddScoped<ICodeQualityService, CodeQualityService>();
        services.AddScoped<ICodeTestsService, CodeTestsService>();

        services.AddScoped<IGitHubBuildService, GitHubBuildService>();
        services.AddScoped<ILanguageDetector, LanguageDetector>();
        services.AddScoped<IProcessService, ProcessService>();
        services.AddScoped<IDockerService, DockerService>();

        services.AddScoped<ICodeAnalyzer, DotNetCodeAnalyzer>();
        services.AddScoped<ICodeAnalyzer, PythonCodeAnalyzer>();
        services.AddScoped<ICodeAnalyzer, JavaCodeAnalyzer>();

        services.AddScoped<ITestsRunner, DotNetTestsRunner>();
        services.AddScoped<ITestsRunner, PythonTestsRunner>();
        services.AddScoped<ITestsRunner, JavaTestsRunner>();
    }
}