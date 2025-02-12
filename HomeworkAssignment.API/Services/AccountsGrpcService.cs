using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using HomeAssignment.DTOs.RequestDTOs;
using HomeworkAssignment.Services.Abstractions;
using RepoAnalisys.Grpc;

namespace HomeworkAssignment.Services;

public class AccountGrpcService : IAccountGrpcService
{
    private readonly AccountsOperator.AccountsOperatorClient _client;
    private readonly ILogger<AccountGrpcService> _logger;
    private readonly IAuthenticationService _authentication; 

    public AccountGrpcService(AccountsOperator.AccountsOperatorClient client, ILogger<AccountGrpcService> logger, IAuthenticationService authentication)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _authentication = authentication ?? throw new ArgumentNullException(nameof(authentication));
    }

    public async Task<IReadOnlyList<string>?> GetBranchesAsync(RequestBranchDto query,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(
            "Getting branches for repo: {RepoTitle}, owner: {OwnerGithubUsername}, author: {AuthorGithubUsername}",
            query.RepoTitle, query.OwnerGitHubUsername, query.AuthorGitHubUsername);

        var request = new BranchQuery
        {
            RepoTitle = query.RepoTitle,
            OwnerGithubUsername = query.OwnerGitHubUsername,
            AuthorGithubUsername = query.AuthorGitHubUsername,
            Since = query.Since.HasValue ? Timestamp.FromDateTime(query.Since.Value.ToUniversalTime()) : null,
            Until = query.Until.HasValue ? Timestamp.FromDateTime(query.Until.Value.ToUniversalTime()) : null
        };

        _logger.LogInformation("Sending request to accounts operator client to get author branches.");
        
        var accessToken = await _authentication.GetAccessTokenAsync(cancellationToken);
        var headers = new Metadata
        {
            { "Authorization", $"Bearer {accessToken}" }
        };

        var response = await _client.GetAuthorBranchesAsync(request, headers, cancellationToken: cancellationToken);

        _logger.LogInformation("Received {BranchCount} branches for repo: {RepoTitle}.", response.BranchTitles.Count,
            query.RepoTitle);

        return response.BranchTitles.ToList();
    }
}