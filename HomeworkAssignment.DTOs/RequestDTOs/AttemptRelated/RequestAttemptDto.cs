namespace HomeAssignment.DTOs.RequestDTOs.AttemptRelated;

public class RequestAttemptDto
{
    public Guid UserId { get; set; }
    public Guid AssignmentId { get; set; }
    
    public ushort Position { get; set; }
    public string? BranchName { get; set; }
    
    
    public ushort FinalScore { get; set; }
    
    public ushort CompilationScore { get; set; } 
    public ushort QualityScore { get; set; }
    public ushort TestsScore { get; set; } 
    
    public bool IsCompleted { get; set; }
}