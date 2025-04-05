namespace HomeAssignment.DTOs.RespondDTOs;

public class RespondCourseWithCategoryDto
{
    public Guid Id { get; init; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    
    public bool IsPublished { get; set; } 

    public RespondCategoryDto? Category { get; set; }
    public required Guid UserId { get; set; }
}