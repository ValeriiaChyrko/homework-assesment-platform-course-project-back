namespace HomeAssignment.DTOs.RequestDTOs;

public class RequestPartialAttemptDto
{
    public Guid UserId { get; set; }
    public Guid? AssignmentId { get; set; }
    
    public int? Position { get; set; }
    public string? BranchName { get; set; }
}