using HomeAssignment.DTOs.RespondDTOs;

namespace HomeAssignment.DTOs.RequestDTOs;

public class RequestSubmitAttemptDto
{ 
    public required Guid UserId { get; set; }
    public required string AuthorGitHubUsername { get; init; } = string.Empty;
    public required RequestAttemptDto Attempt { get; set; }
    public required RespondAssignmentDto Assignment { get; set; }
}