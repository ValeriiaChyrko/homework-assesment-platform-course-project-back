namespace HomeAssignment.Database.Entities;

public class EnrollmentEntity
{
    public Guid Id { get; set; }
   
    public required Guid UserId { get; init; }
    public UserEntity? User { get; init; }
    
    public required Guid CourseId { get; init; }
    public CourseEntity? Course { get; init; }
    
    public DateTime CreatedAt { get; init; } 
    public DateTime UpdatedAt { get; init; } 
}