namespace HomeAssignment.Domain.Abstractions;

public class Course
{
    private readonly List<Guid> _attachmentIds;
    private readonly List<Guid> _enrollmentIds;

    public Course(Guid id, string title, string? description, string? imageUrl, bool isPublished, 
        Guid userId, Guid? categoryId, List<Guid>? attachmentIds, List<Guid>? enrollmentIds,
        DateTime createdAt, DateTime updatedAt)
    {
        Id = id;
        Title = title;
        Description = description;
        ImageUrl = imageUrl;
        IsPublished = isPublished;
        UserId = userId;
        CategoryId = categoryId;
        _attachmentIds = attachmentIds ?? [];
        _enrollmentIds = enrollmentIds ?? [];
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    public Guid Id { get; init; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsPublished { get; set; }

    public Guid UserId { get; set; }
    public Guid? CategoryId { get; set; }

    public IReadOnlyList<Guid> AttachmentIds => _attachmentIds.AsReadOnly();

    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; set; }

    public static Course Create(string title, string? description, string? imageUrl, bool isPublished, 
        Guid userId, Guid? categoryId, List<Guid>? attachmentIds = null, List<Guid>? enrollmentIds = null)
    {
        var courseId = Guid.NewGuid();
        var createdAt = DateTime.UtcNow;

        return new Course(
            courseId,
            title,
            description,
            imageUrl,
            isPublished,
            userId,
            categoryId,
            attachmentIds ?? [],
            enrollmentIds ?? [],
            createdAt,
            createdAt
        );
    }

    public void Update(string title, string? description, string? imageUrl, 
        Guid userId, Guid? categoryId)
    {
        Title = title;
        Description = description;
        ImageUrl = imageUrl;
        UserId = userId;
        CategoryId = categoryId;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Publish()
    {
        IsPublished = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void AddAttachment(Guid attachmentId)
    {
        if (_attachmentIds.Contains(attachmentId)) return;
        
        _attachmentIds.Add(attachmentId);
        UpdatedAt = DateTime.UtcNow;
    }

    public void RemoveAttachment(Guid attachmentId)
    {
        if (_attachmentIds.Remove(attachmentId))
        {
            UpdatedAt = DateTime.UtcNow;
        }
    }
    
    public void AddEnrollment(Guid enrollmentId)
    {
        if (_enrollmentIds.Contains(enrollmentId)) return;
        
        _enrollmentIds.Add(enrollmentId);
        UpdatedAt = DateTime.UtcNow;
    }

    public void RemoveEnrollment(Guid enrollmentId)
    {
        if (_enrollmentIds.Remove(enrollmentId))
        {
            UpdatedAt = DateTime.UtcNow;
        }
    }
}