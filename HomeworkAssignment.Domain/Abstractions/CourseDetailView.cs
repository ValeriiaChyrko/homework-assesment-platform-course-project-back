namespace HomeAssignment.Domain.Abstractions;

public class CourseDetailView(
    Guid id,
    string title,
    string? description,
    string? imageUrl,
    bool isPublished,
    Guid userId,
    Guid? categoryId,
    DateTime createdAt,
    DateTime updatedAt,
    List<Chapter> chapters,
    List<Attachment> attachments,
    Category? category)
{
    public Guid Id { get; init; } = id;
    public string Title { get; init; } = title;
    public string? Description { get; init; } = description;
    public string? ImageUrl { get; init; } = imageUrl;
    public bool IsPublished { get; init; } = isPublished;
    public Guid UserId { get; init; } = userId;
    public Guid? CategoryId { get; init; } = categoryId;
    public DateTime CreatedAt { get; init; } = createdAt;
    public DateTime UpdatedAt { get; init; } = updatedAt;

    public IReadOnlyList<Chapter> Chapters { get; init; } = chapters;
    public IReadOnlyList<Attachment> Attachments { get; init; } = attachments;
    public Category? Category { get; init; } = category;
}