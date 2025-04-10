using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using HomeAssignment.DTOs.RequestDTOs;
using HomeAssignment.DTOs.RequestDTOs.AttemptRelated;
using HomeworkAssignment.Services.Abstractions;
using RepoAnalisys.Grpc;

namespace HomeworkAssignment.Services;

public class AccountGrpcService : IAccountGrpcService
{
    private readonly AccountsOperator.AccountsOperatorClient _client;
    private readonly IKeycloakTokenService _keycloakTokenService;
    private readonly ILogger<AccountGrpcService> _logger;

    public AccountGrpcService(AccountsOperator.AccountsOperatorClient client, ILogger<AccountGrpcService> logger,
        IKeycloakTokenService keycloakTokenService)
    {
        _client = client;
        _logger = logger;
        _keycloakTokenService = keycloakTokenService;
    }

    public async Task<IReadOnlyList<string>?> GetBranchesAsync(RequestBranchDto query,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(
            "Getting branches for repo: {RepoTitle}, owner: {OwnerGithubUsername}, author: {AuthorGithubUsername}",
            query.RepoTitle, query.OwnerGitHubUsername, query.AuthorGitHubUsername);

        var accessToken = await _keycloakTokenService.GetAccessTokenAsync();
        if (string.IsNullOrEmpty(accessToken))
            throw new RpcException(new Status(StatusCode.Unauthenticated, "Failed to get access token"));

        var metadata = new Metadata { { "Authorization", $"Bearer {accessToken}" } };

        var request = new BranchQuery
        {
            RepoTitle = query.RepoTitle,
            OwnerGithubUsername = query.OwnerGitHubUsername,
            AuthorGithubUsername = query.AuthorGitHubUsername,
            Since = query.Since.HasValue ? Timestamp.FromDateTime(query.Since.Value.ToUniversalTime()) : null,
            Until = query.Until.HasValue ? Timestamp.FromDateTime(query.Until.Value.ToUniversalTime()) : null
        };

        _logger.LogInformation("Sending request to accounts operator client to get author branches.");

        var response = await _client.GetAuthorBranchesAsync(request, metadata, cancellationToken: cancellationToken);

        _logger.LogInformation("Received {BranchCount} branches for repo: {RepoTitle}.", response.BranchTitles.Count,
            query.RepoTitle);

        return response.BranchTitles.ToList();
    }
}