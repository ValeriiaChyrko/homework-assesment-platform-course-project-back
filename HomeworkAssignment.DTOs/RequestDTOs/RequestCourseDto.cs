namespace HomeAssignment.DTOs.RequestDTOs;

public class RequestCourseDto
{
    public required string Title { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    
    public bool IsPublished { get; set; } 

    public Guid? CategoryId { get; set; }
    public required Guid UserId { get; set; }
}