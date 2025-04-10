using HomeAssignment.DTOs.RespondDTOs.CategoryRelated;
using HomeAssignment.DTOs.RespondDTOs.ChapterRelated;

namespace HomeAssignment.DTOs.RespondDTOs.CourseRelated;

public class RespondCourseWithProgressWithCategoryDto
{
    public Guid Id { get; init; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    
    public bool IsPublished { get; set; } 
    public int Progress { get; set; } 

    public required RespondCategoryDto Category { get; set; }
    public required IReadOnlyCollection<RespondChapterDto> Chapters { get; set; }
    public Guid UserId { get; set; }
}