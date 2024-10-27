namespace HomeAssignment.DTOs.SharedDTOs;

public class GitHubProfileDto
{
    public Guid Id { get; set; }
    public required string GithubUsername { get; set; }
    public required string GithubAccessToken { get; set; }
    public required string GithubProfileUrl { get; set; }
    public string? GithubPictureUrl { get; set; }
}