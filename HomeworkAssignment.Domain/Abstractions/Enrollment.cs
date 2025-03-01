namespace HomeAssignment.Domain.Abstractions;

public class Enrollment
{
    public Enrollment(Guid id, Guid userId, Guid? courseId, DateTime createdAt, DateTime updatedAt)
    {
        Id = id;
        UserId = userId;
        CourseId = courseId;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    public Guid Id { get; init; }
   
    public Guid UserId { get; set; }
    public Guid? CourseId { get; set; }
    
    public DateTime CreatedAt { get; init; } 
    public DateTime UpdatedAt { get; set; }

    public static Enrollment Create(Guid userId, Guid courseId)
    {
        return new Enrollment(
            Guid.NewGuid(), 
            userId,
            courseId,
            DateTime.UtcNow,
            DateTime.UtcNow
        );
    }

    public void Update(Guid userId, Guid courseId)
    {
        UserId = userId;
        CourseId = courseId;
        UpdatedAt = DateTime.UtcNow;
    }
}