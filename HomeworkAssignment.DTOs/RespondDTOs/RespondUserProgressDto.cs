namespace HomeAssignment.DTOs.RespondDTOs;

public class RespondUserProgressDto
{
    public Guid Id { get; init; }
    public bool IsCompleted { get; set; }
    
    public Guid UserId { get; set; }
    public Guid ChapterId { get; set; }
}