namespace HomeAssignment.DTOs.RespondDTOs;

public class RespondCourseWithCategoryWithProgressDto
{
    public Guid Id { get; init; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    
    public bool IsPublished { get; set; } 
    public int Progress { get; set; } 
    
    public bool IsEnrolled { get; set; } = false;

    public RespondCategoryDto? Category { get; set; }
    public required List<RespondChapterWithUserProgressDto> Chapters { get; set; }
    public required Guid UserId { get; set; }
}