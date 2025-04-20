using Grpc.Core;
using HomeAssignment.DTOs.RequestDTOs.AttemptRelated;
using HomeworkAssignment.Services.Abstractions;
using RepoAnalisys.Grpc;

namespace HomeworkAssignment.Services;

public class TestsGrpcService : ITestsGrpcService
{
    private readonly TestsOperator.TestsOperatorClient _client;
    private readonly IKeycloakTokenService _keycloakTokenService;
    private readonly ILogger<TestsGrpcService> _logger;

    public TestsGrpcService(TestsOperator.TestsOperatorClient client, ILogger<TestsGrpcService> logger,
        IKeycloakTokenService keycloakTokenService)
    {
        _client = client;
        _logger = logger;
        _keycloakTokenService = keycloakTokenService;
    }

    public async Task<int> VerifyProjectPassedTestsAsync(RequestRepositoryWithBranchDto query,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(
            "Verifying tests for repo: {RepoTitle}, branch: {BranchTitle}, owner: {OwnerGithubUsername}, author: {AuthorGithubUsername}",
            query.RepoTitle, query.BranchTitle, query.OwnerGitHubUsername, query.AuthorGitHubUsername);

        var accessToken = await _keycloakTokenService.GetAccessTokenAsync();
        if (string.IsNullOrEmpty(accessToken))
            throw new RpcException(new Status(StatusCode.Unauthenticated, "Failed to get access token"));

        var metadata = new Metadata { { "Authorization", $"Bearer {accessToken}" } };

        var request = new RepositoryWithBranchQuery
        {
            RepoTitle = query.RepoTitle,
            BranchTitle = query.BranchTitle,
            OwnerGithubUsername = query.OwnerGitHubUsername,
            AuthorGithubUsername = query.AuthorGitHubUsername
        };

        _logger.LogInformation("Sending request to tests operator client to verify project tests.");

        var response =
            await _client.VerifyProjectPassedTestsAsync(request, metadata, cancellationToken: cancellationToken);

        _logger.LogInformation("Received tests score: {Score} for repo: {RepoTitle}, branch: {BranchTitle}.",
            response.Score, query.RepoTitle, query.BranchTitle);

        return response.Score;
    }
}