namespace HomeAssignment.DTOs.RequestDTOs;

public class RequestAssignmentUserProgressDto
{
    public bool IsCompleted { get; set; }
    
    public Guid UserId { get; set; }
    public Guid AssignmentId { get; set; }
}