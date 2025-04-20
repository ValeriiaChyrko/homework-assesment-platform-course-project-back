namespace HomeAssignment.Database.Entities;

public class AttachmentEntity
{
    public Guid Id { get; set; }
    public required string Name { get; init; }
    public required string Url { get; init; }
    public required string UploadthingKey { get; init; }

    public Guid? CourseId { get; init; }
    public CourseEntity? Course { get; init; }

    public Guid? ChapterId { get; init; }
    public ChapterEntity? Chapter { get; init; }

    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
}