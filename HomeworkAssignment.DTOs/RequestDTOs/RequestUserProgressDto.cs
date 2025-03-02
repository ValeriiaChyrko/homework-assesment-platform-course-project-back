namespace HomeAssignment.DTOs.RequestDTOs;

public class RequestUserProgressDto
{
    public bool IsCompleted { get; set; }
    
    public Guid UserId { get; set; }
    public Guid ChapterId { get; set; }
}