namespace HomeAssignment.Database.Entities;

public class UserProgressEntity
{
    public Guid Id { get; set; }
    
    public bool IsCompleted { get; set; } = false;
    
    public Guid UserId { get; set; }
    public UserEntity User { get; set; }
    
    public Guid? ChapterId { get; set; }
    public ChapterEntity? Chapter { get; set; }

    public DateTime CreatedAt { get; set; } 
    public DateTime UpdatedAt { get; set; } 
}