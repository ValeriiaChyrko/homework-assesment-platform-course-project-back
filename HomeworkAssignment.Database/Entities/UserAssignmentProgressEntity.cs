namespace HomeAssignment.Database.Entities;

public class UserAssignmentProgressEntity
{
    public Guid Id { get; init; }
    
    public bool IsCompleted { get; init; } 

    public DateTime CreatedAt { get; init; } 
    public DateTime UpdatedAt { get; init; } 

    public Guid UserId { get; init; }
    public required UserEntity User { get; init; }

    public Guid AssignmentId { get; init; }
    public required AssignmentEntity Assignment { get; init; }
}
