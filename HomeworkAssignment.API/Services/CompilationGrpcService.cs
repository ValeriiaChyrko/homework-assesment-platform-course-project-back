using Grpc.Core;
using HomeAssignment.DTOs.RequestDTOs;
using HomeworkAssignment.Services.Abstractions;
using RepoAnalisys.Grpc;

namespace HomeworkAssignment.Services;

public class CompilationGrpcService : ICompilationGrpcService
{
    private readonly CompilationOperator.CompilationOperatorClient _client;
    private readonly ILogger<CompilationGrpcService> _logger;
    private readonly IAuthenticationService _authentication; 

    public CompilationGrpcService(CompilationOperator.CompilationOperatorClient client, ILogger<CompilationGrpcService> logger, IAuthenticationService authentication)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _authentication = authentication ?? throw new ArgumentNullException(nameof(authentication));
    }

    public async Task<int> VerifyProjectCompilation(RequestRepositoryWithBranchDto query,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(
            "Verifying compilation for repo: {RepoTitle}, branch: {BranchTitle}, owner: {OwnerGithubUsername}, author: {AuthorGithubUsername}",
            query.RepoTitle, query.BranchTitle, query.OwnerGitHubUsername, query.AuthorGitHubUsername);

        var request = new RepositoryWithBranchQuery
        {
            RepoTitle = query.RepoTitle,
            BranchTitle = query.BranchTitle,
            OwnerGithubUsername = query.OwnerGitHubUsername,
            AuthorGithubUsername = query.AuthorGitHubUsername
        };

        _logger.LogInformation("Sending request to compilation operator client to verify project compilation.");
        
        var accessToken = await _authentication.GetAccessTokenAsync(cancellationToken);
        var headers = new Metadata
        {
            { "Authorization", $"Bearer {accessToken}" }
        };

        var response = await _client.VerifyProjectCompilationAsync(request, headers, cancellationToken: cancellationToken);

        _logger.LogInformation("Received compilation score: {Score} for repo: {RepoTitle}, branch: {BranchTitle}.",
            response.Score, query.RepoTitle, query.BranchTitle);

        return response.Score;
    }
}