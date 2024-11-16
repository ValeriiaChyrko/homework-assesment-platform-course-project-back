namespace HomeAssignment.Database.Entities;

public class AttemptEntity
{
    public Guid Id { get; set; }
    public int AttemptNumber { get; set; }
    public string BranchName { get; set; } = string.Empty;
    public DateTime FinishedAt { get; set; }

    public int CompilationScore { get; set; }
    public int TestsScore { get; set; }
    public int QualityScore { get; set; }
    public int FinalScore { get; set; }

    public Guid StudentId { get; set; }
    public GitHubProfilesEntity Student { get; set; } = null!;

    public Guid AssignmentId { get; set; }
    public AssignmentEntity Assignment { get; set; } = null!;
}