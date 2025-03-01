namespace HomeAssignment.Domain.Abstractions;

public class UserProgress
{
    public UserProgress(Guid id, bool isCompleted, Guid userId, Guid? chapterId, DateTime createdAt, DateTime updatedAt)
    {
        Id = id;
        IsCompleted = isCompleted;
        UserId = userId;
        ChapterId = chapterId;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    public Guid Id { get; set; }
    
    public bool IsCompleted { get; set; } 
    
    public Guid UserId { get; set; }
    public Guid? ChapterId { get; set; }

    public DateTime CreatedAt { get; set; } 
    public DateTime UpdatedAt { get; set; }

    public static UserProgress Create(bool isCompleted, Guid userId, Guid chapterId)
    {
        return new UserProgress(
            Guid.NewGuid(), 
            isCompleted,
            userId,
            chapterId,
            DateTime.UtcNow,
            DateTime.UtcNow
        );
    }

    public void Complete()
    {
        IsCompleted = true;
        UpdatedAt = DateTime.UtcNow;
    }
}