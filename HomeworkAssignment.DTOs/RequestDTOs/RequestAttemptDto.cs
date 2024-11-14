namespace HomeAssignment.DTOs.RequestDTOs;

public class RequestAttemptDto
{
    public Guid StudentId { get; init; }
    public Guid AssignmentId { get; init; }
    public int AttemptNumber { get; init; }
    public string? BranchName { get; init; }
    public int CompilationScore { get; init; }
    public int TestsScore { get; init; }
    public int QualityScore { get; init; }
}