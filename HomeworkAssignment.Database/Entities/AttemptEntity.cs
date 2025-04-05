namespace HomeAssignment.Database.Entities;

using System;

public class AttemptEntity
{
    public Guid Id { get; set; } 
    public ushort Position { get; init; }

    public string? BranchName { get; init; }
    
    public ushort FinalScore { get; set; }
    public ushort CompilationScore { get; set; } 
    public ushort QualityScore { get; set; }
    public ushort TestsScore { get; set; }

    public bool IsCompleted { get; init; } 

    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }

    public Guid UserId { get; init; }
    public required UserEntity User { get; init; }

    public Guid AssignmentId { get; init; }
    public required AssignmentEntity Assignment { get; init; }
}
