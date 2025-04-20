namespace HomeAssignment.Database.Entities;

public class CourseEntity
{
    public Guid Id { get; set; }
    public required string Title { get; init; }
    public string? Description { get; init; }
    public string? ImageUrl { get; init; }
    public bool IsPublished { get; init; }

    public Guid? CategoryId { get; init; }
    public CategoryEntity? Category { get; init; }

    public ICollection<ChapterEntity>? Chapters { get; init; }
    public ICollection<AttachmentEntity>? Attachments { get; init; }
    public ICollection<EnrollmentEntity>? Enrollments { get; init; }

    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }

    public required Guid UserId { get; init; }
    public required UserEntity User { get; init; }
}