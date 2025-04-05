namespace HomeAssignment.DTOs.RespondDTOs;

public class RespondCourseWithChapters
{
    public Guid Id { get; init; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    
    public bool IsPublished { get; set; } 
    
    public required List<RespondChapterDto> Chapters { get; set; }
    public required Guid UserId { get; set; }
}