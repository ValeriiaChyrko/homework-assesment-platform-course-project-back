using HomeAssignment.DTOs.RespondDTOs.AssignmentRelated;

namespace HomeAssignment.DTOs.RequestDTOs.AttemptRelated;

public class RequestSubmitAttemptDto
{
    public required string AuthorGitHubUsername { get; init; } = string.Empty;
    public required AttemptDto Attempt { get; set; }
    public required RespondAssignmentDto Assignment { get; set; }
}