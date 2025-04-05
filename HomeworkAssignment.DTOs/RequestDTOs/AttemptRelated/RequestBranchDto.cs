namespace HomeAssignment.DTOs.RequestDTOs;

public class RequestBranchDto
{
    public required string RepoTitle { get; set; } = string.Empty;
    public string? OwnerGitHubUsername { get; set; } = string.Empty;
    public required Guid UserId { get; set; }
    public required string AuthorGitHubUsername { get; init; } = string.Empty;
    public DateTime? Since { get; init; }
    public DateTime? Until { get; init; }
}