namespace HomeAssignment.Database.Entities;

public class AttachmentEntity
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Url { get; set; }

    public Guid? CourseId { get; set; }
    public CourseEntity? Course { get; set; }
    
    public Guid? ChapterId { get; set; }
    public ChapterEntity? Chapter { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; } 
}