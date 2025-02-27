namespace HomeAssignment.Database.Entities;

using System;

public class AttemptProgressEntity
{
    public Guid Id { get; set; } 
    public int Position { get; set; }

    public string? BranchName { get; set; }
    
    public int FinalScore { get; set; } = 0;
    
    public int CompilationScore { get; set; } = 0;
    public int QualityScore { get; set; } = 0;
    public int TestsScore { get; set; } = 0;

    public required string ProgressStatus { get; set; }
    public bool IsCompleted { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    public Guid UserId { get; set; }
    public required UserEntity User { get; set; }

    public Guid AssignmentId { get; set; }
    public required AssignmentEntity Assignment { get; set; }
}
