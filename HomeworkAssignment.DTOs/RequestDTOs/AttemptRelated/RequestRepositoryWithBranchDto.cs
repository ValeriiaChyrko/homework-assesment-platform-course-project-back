namespace HomeAssignment.DTOs.RequestDTOs.AttemptRelated;

public class RequestRepositoryWithBranchDto
{
    public string RepoTitle { get; init; } = string.Empty;
    public string OwnerGitHubUsername { get; init; } = string.Empty;
    public string AuthorGitHubUsername { get; init; } = string.Empty;
    public string BranchTitle { get; init; } = string.Empty;
}