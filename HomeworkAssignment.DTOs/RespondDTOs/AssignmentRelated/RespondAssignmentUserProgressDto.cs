namespace HomeAssignment.DTOs.RespondDTOs.AssignmentRelated;

public class RespondAssignmentUserProgressDto
{
    public Guid Id { get; init; }
    public bool IsCompleted { get; set; }
    
    public Guid UserId { get; set; }
    public Guid AssignmentId { get; set; }
}