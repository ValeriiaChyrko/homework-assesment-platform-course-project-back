using Google.Protobuf.WellKnownTypes;
using HomeAssignment.DTOs.RequestDTOs;
using HomeworkAssignment.Services.Abstractions;
using RepoAnalisys.Grpc;

namespace HomeworkAssignment.Services;

public class AccountGrpcService : IAccountGrpcService
{
    private readonly AccountsOperator.AccountsOperatorClient _client;
    private readonly ILogger<AccountGrpcService> _logger;

    public AccountGrpcService(AccountsOperator.AccountsOperatorClient client, ILogger<AccountGrpcService> logger)
    {
        _client = client;
        _logger = logger;
    }

    public async Task<IReadOnlyList<string>?> GetBranchesAsync(RequestBranchDto query, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting branches for repo: {RepoTitle}, owner: {OwnerGithubUsername}, author: {AuthorGithubUsername}", 
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

        var response = await _client.GetAuthorBranchesAsync(request, cancellationToken: cancellationToken);

        _logger.LogInformation("Received {BranchCount} branches for repo: {RepoTitle}.", response.BranchTitles.Count, query.RepoTitle);

        return response.BranchTitles.ToList();
    }
}
