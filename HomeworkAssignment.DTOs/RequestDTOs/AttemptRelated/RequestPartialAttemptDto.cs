namespace HomeAssignment.DTOs.RequestDTOs.AttemptRelated;

public class RequestPartialAttemptDto
{
    public Guid UserId { get; set; }
    public Guid? AssignmentId { get; set; }
    
    public ushort? Position { get; set; }
    public string? BranchName { get; set; }
}