namespace HomeAssignment.Database.Entities;

public class CourseEntity
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsPublished { get; set; } = false;

    public Guid? CategoryId { get; set; }
    public CategoryEntity? Category { get; set; }
    
    public ICollection<ChapterEntity>? Chapters { get; set; }
    public ICollection<AttachmentEntity>? Attachments { get; set; }
    public ICollection<EnrollmentEntity>? Enrollments { get; set; }

    public DateTime CreatedAt { get; set; } 
    public DateTime UpdatedAt { get; set; } 
    
    public required Guid UserId { get; set; }
    public required UserEntity User { get; set; }
}