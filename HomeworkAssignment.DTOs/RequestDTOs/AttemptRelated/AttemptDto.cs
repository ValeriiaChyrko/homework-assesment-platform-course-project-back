namespace HomeAssignment.DTOs.RequestDTOs.AttemptRelated;

public class AttemptDto
{
    public ushort Position { get; set; }
    public required string BranchName { get; set; }

    public ushort CompilationScore { get; set; }
    public ushort QualityScore { get; set; }
    public ushort TestsScore { get; set; }

    public bool IsCompleted { get; set; }
}