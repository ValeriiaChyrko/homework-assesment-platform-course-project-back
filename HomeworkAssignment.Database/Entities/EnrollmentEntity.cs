namespace HomeAssignment.Database.Entities;

public class EnrollmentEntity
{
    public Guid Id { get; set; }
   
    public required Guid UserId { get; set; }
    public UserEntity? User { get; set; }
    
    public Guid? CourseId { get; set; }
    public CourseEntity? Course { get; set; }
    
    public DateTime CreatedAt { get; set; } 
    public DateTime UpdatedAt { get; set; } 
}