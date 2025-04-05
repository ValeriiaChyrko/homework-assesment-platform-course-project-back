namespace HomeAssignment.Database.Entities;

public class UserChapterProgressEntity
{
    public Guid Id { get; init; }
    
    public bool IsCompleted { get; init; }
    
    public Guid UserId { get; init; }
    public UserEntity User { get; init; }
    
    public Guid? ChapterId { get; init; }
    public ChapterEntity? Chapter { get; init; }

    public DateTime CreatedAt { get; init; } 
    public DateTime UpdatedAt { get; init; } 
}