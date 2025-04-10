namespace HomeAssignment.DTOs.RespondDTOs.ChapterRelated;

public class RespondChapterDto
{
    public Guid Id { get; init; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public string? VideoUrl { get; set; }
    
    public int Position { get; set; }
    
    public bool IsPublished { get; set; } 
    public bool IsFree { get; set; } 
    
    public Guid? MuxDataId { get; set; }
    public Guid? CourseId { get; set; }
}