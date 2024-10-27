namespace HomeAssignment.Domain.Abstractions.Contracts;

public interface IHaveGitHubProfile
{
    public Guid GitHubProfileId { get; set; }
    public string GithubUsername { get; set; }
    public string GithubAccessToken { get; set; }
    public string GithubProfileUrl { get; set; }
    public string? GithubPictureUrl { get; set; }
}