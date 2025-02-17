namespace HomeAssignment.DTOs.RequestDTOs;

public class RequestBranchDto
{
    public string RepoTitle { get; init; } = string.Empty;
    public string OwnerGitHubUsername { get; init; } = string.Empty;
    public string AuthorGitHubUsername { get; init; } = string.Empty;
    public DateTime? Since { get; init; }
    public DateTime? Until { get; init; }
}