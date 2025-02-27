namespace HomeAssignment.Database.Entities;

public class ChapterEntity
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public string? VideoUrl { get; set; }
    public int Position { get; set; }
    public bool IsPublished { get; set; } = false;
    public bool IsFree { get; set; } = false;
    
    public Guid? MuxDataId { get; set; }
    public MuxDataEntity? MuxData { get; set; }

    public Guid? CourseId { get; set; }
    public CourseEntity? Course { get; set; }
    
    public ICollection<UserProgressEntity>? UsersProgress { get; set; }
    public ICollection<AttachmentEntity>? Attachments { get; set; }
    public ICollection<AssignmentEntity>? Assignments { get; set; }

    public DateTime CreatedAt { get; set; } 
    public DateTime UpdatedAt { get; set; } 
}