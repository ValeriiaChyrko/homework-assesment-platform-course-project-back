namespace HomeAssignment.DTOs.RequestDTOs.AttemptRelated;

public class RequestBranchDto
{
    public required string RepoTitle { get; set; }
    public required string BaseBranch { get; set; }
    public required string OwnerGitHubUsername { get; set; }
    public required string AuthorGitHubUsername { get; init; }
    public DateTime? Since { get; init; }
    public DateTime? Until { get; init; }
}