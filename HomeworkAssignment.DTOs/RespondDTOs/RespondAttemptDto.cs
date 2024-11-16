namespace HomeAssignment.DTOs.RespondDTOs;

public class RespondAttemptDto
{
    public Guid Id { get; set; }
    public Guid StudentId { get; init; }
    public Guid AssignmentId { get; init; }
    public string BranchName { get; init; } = string.Empty;
    public DateTime FinishedAt { get; init; }
    public int AttemptNumber { get; init; }
    public int CompilationScore { get; init; }
    public int TestsScore { get; init; }
    public int QualityScore { get; init; }
    public int FinalScore { get; init; }
}