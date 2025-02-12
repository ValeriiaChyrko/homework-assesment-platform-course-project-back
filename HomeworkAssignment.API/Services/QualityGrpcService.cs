using Grpc.Core;
using HomeAssignment.DTOs.RequestDTOs;
using HomeworkAssignment.Services.Abstractions;
using RepoAnalisys.Grpc;

namespace HomeworkAssignment.Services;

public class QualityGrpcService : IQualityGrpcService
{
    private readonly QualityOperator.QualityOperatorClient _client;
    private readonly ILogger<QualityGrpcService> _logger;
    private readonly IAuthenticationService _authentication; 

    public QualityGrpcService(QualityOperator.QualityOperatorClient client, ILogger<QualityGrpcService> logger, IAuthenticationService authentication)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _authentication = authentication ?? throw new ArgumentNullException(nameof(authentication));
    }

    public async Task<int> VerifyProjectQualityAsync(RequestRepositoryWithBranchDto query,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(
            "Verifying quality for repo: {RepoTitle}, branch: {BranchTitle}, owner: {OwnerGithubUsername}, author: {AuthorGithubUsername}",
            query.RepoTitle, query.BranchTitle, query.OwnerGitHubUsername, query.AuthorGitHubUsername);

        var request = new RepositoryWithBranchQuery
        {
            RepoTitle = query.RepoTitle,
            BranchTitle = query.BranchTitle,
            OwnerGithubUsername = query.OwnerGitHubUsername,
            AuthorGithubUsername = query.AuthorGitHubUsername
        };

        _logger.LogInformation("Sending request to quality operator client to verify project quality.");
        
        var accessToken = await _authentication.GetAccessTokenAsync(cancellationToken);
        var headers = new Metadata
        {
            { "Authorization", $"Bearer {accessToken}" }
        };

        var response = await _client.VerifyProjectQualityAsync(request, headers, cancellationToken: cancellationToken);

        _logger.LogInformation("Received project quality score: {Score} for repo: {RepoTitle}, branch: {BranchTitle}.",
            response.Score, query.RepoTitle, query.BranchTitle);

        return response.Score;
    }
}