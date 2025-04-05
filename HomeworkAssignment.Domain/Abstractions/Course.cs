namespace HomeAssignment.Domain.Abstractions;

public class Course(
    Guid id,
    string title,
    string? description,
    string? imageUrl,
    bool isPublished,
    Guid userId,
    Guid? categoryId,
    List<Guid>? attachmentIds,
    DateTime createdAt,
    DateTime updatedAt)
{
    private readonly List<Guid> _attachmentIds = attachmentIds ?? [];

    public Guid Id { get; init; } = id;
    public string Title { get; set; } = title;
    public string? Description { get; set; } = description;
    public string? ImageUrl { get; set; } = imageUrl;
    public bool IsPublished { get; private set; } = isPublished;
    public Guid UserId { get; set; } = userId;
    public Guid? CategoryId { get; set; } = categoryId;
    public IReadOnlyList<Guid> AttachmentIds => _attachmentIds.AsReadOnly();
    public DateTime CreatedAt { get; init; } = createdAt;
    public DateTime UpdatedAt { get; private set; } = updatedAt;

    public static Course Create(string title)
    {
        var now = DateTime.UtcNow;
        return new Course(
            Guid.NewGuid(),
            title,
            null,
            null,
            false,
            Guid.Empty,
            null,
            [],
            now,
            now
        );
    }

    public void PatchUpdate(
        string? title = null,
        string? description = null,
        string? imageUrl = null,
        Guid? userId = null,
        Guid? categoryId = null)
    {
        Title = title ?? Title;
        Description = description ?? Description;
        ImageUrl = imageUrl ?? ImageUrl;
        UserId = userId ?? UserId;
        CategoryId = categoryId ?? CategoryId;

        UpdatedAt = DateTime.UtcNow;
    }

    public void MarkAsPublished()
    {
        IsPublished = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void MarkAsUnpublished()
    {
        IsPublished = false;
        UpdatedAt = DateTime.UtcNow;
    }
}