namespace HomeAssignment.Domain.Abstractions;

public class Chapter
{
    private readonly List<Guid> _attachmentIds;
    private readonly List<Guid> _userProgressIds;
    
    public Chapter(Guid id, string title, string? description, string? videoUrl, int position, bool isPublished, bool isFree, 
        Guid? muxDataId, Guid? courseId, List<Guid>? attachmentIds, List<Guid>? userProgressIds, DateTime createdAt, DateTime updatedAt)
    {
        Id = id;
        Title = title;
        Description = description;
        VideoUrl = videoUrl;
        Position = position;
        IsPublished = isPublished;
        IsFree = isFree;
        MuxDataId = muxDataId;
        CourseId = courseId;
        _attachmentIds = attachmentIds ?? [];
        _userProgressIds = userProgressIds ?? [];
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    public Guid Id { get; init; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public string? VideoUrl { get; set; }
    public int Position { get; set; }
    
    public bool IsPublished { get; set; }
    public bool IsFree { get; set; } 
    
    public Guid? MuxDataId { get; set; }
    public Guid? CourseId { get; set; }
    public IReadOnlyList<Guid> AttachmentIds => _attachmentIds.AsReadOnly();
    public IReadOnlyList<Guid> UserProgressIds => _userProgressIds.AsReadOnly();
    
    public DateTime CreatedAt { get; init; } 
    public DateTime UpdatedAt { get; set; }

    public static Chapter Create(string title, string? description, string? videoUrl, int position,
        bool isPublished, bool isFree, Guid? muxDataId, Guid? courseId,  
        List<Guid>? attachmentIds = null, List<Guid>? userProgressIds = null)
    {
        return new Chapter(
            Guid.NewGuid(),
            title,
            description,
            videoUrl,
            position,
            isPublished,
            isFree,
            muxDataId,
            courseId,
            attachmentIds ?? [],
            userProgressIds ?? [],
            DateTime.UtcNow,
            DateTime.UtcNow
        );
    }

    public void Update(string title, string? description, string? videoUrl, int position, bool isFree, Guid? muxData, Guid? course)
    {
        Title = title;
        Description = description;
        VideoUrl = videoUrl;
        Position = position;
        
        MuxDataId = muxData;
        CourseId = course;
        
        UpdatedAt = DateTime.UtcNow;
    }

    public void Publish()
    {
       UpdatedAt = DateTime.UtcNow;
        
        IsPublished = true;
    }
    
    public void Unpublish()
    {
        UpdatedAt = DateTime.UtcNow;
        
        IsPublished = false;
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
    
    public void AddUserProgress(Guid userProgressId)
    {
        if (_userProgressIds.Contains(userProgressId)) return;
        
        _userProgressIds.Add(userProgressId);
        UpdatedAt = DateTime.UtcNow;
    }

    public void RemoveUserProgress(Guid userProgressId)
    {
        if (_userProgressIds.Remove(userProgressId))
        {
            UpdatedAt = DateTime.UtcNow;
        }
    }
}