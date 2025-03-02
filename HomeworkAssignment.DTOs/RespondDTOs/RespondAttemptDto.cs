namespace HomeAssignment.DTOs.RespondDTOs;

public class RespondAttemptDto
{
    public Guid Id { get; init; }
    public Guid UserId { get; set; }
    public Guid AssignmentId { get; set; }
    
    public int Position { get; set; }
    public string? BranchName { get; set; }
    
    
    public int FinalScore { get; set; }
    
    public int CompilationScore { get; set; } 
    public int QualityScore { get; set; }
    public int TestsScore { get; set; } 
    
    public bool IsCompleted { get; set; }
    public required string ProgressStatus { get; set; }
}