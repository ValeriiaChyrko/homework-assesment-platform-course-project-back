namespace HomeAssignment.Database.Entities;

public class ChapterEntity
{
    public Guid Id { get; set; }
    public required string Title { get; init; }
    public string? Description { get; init; }
    public string? VideoUrl { get; init; }
    public ushort Position { get; set; }
    public bool IsPublished { get; init; } 
    public bool IsFree { get; init; } 
    
    public Guid? MuxDataId { get; init; }

    public Guid? CourseId { get; init; }
    public CourseEntity? Course { get; init; }
    
    public ICollection<UserChapterProgressEntity>? UsersProgress { get; init; }
    public ICollection<AttachmentEntity>? Attachments { get; init; }
    public ICollection<AssignmentEntity>? Assignments { get; init; }

    public DateTime CreatedAt { get; init; } 
    public DateTime UpdatedAt { get; set; } 
}