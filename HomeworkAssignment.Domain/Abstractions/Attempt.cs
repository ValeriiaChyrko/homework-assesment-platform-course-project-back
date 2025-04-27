namespace HomeAssignment.Domain.Abstractions;

public class Attempt(
    Guid id,
    ushort position,
    string? branchName,
    bool isCompleted,
    DateTime createdAt,
    DateTime updatedAt,
    Guid userId,
    Guid assignmentId)
{
    public Guid Id { get; init; } = id;
    public ushort Position { get; set; } = position;
    public string? BranchName { get; set; } = branchName;
    public ushort FinalScore { get; private set; }
    public ushort CompilationScore { get; private set; }
    public ushort QualityScore { get; private set; }
    public ushort TestsScore { get; private set; }
    public bool IsCompleted { get; set; } = isCompleted;
    public DateTime CreatedAt { get; } = createdAt;
    public DateTime UpdatedAt { get; set; } = updatedAt;
    public Guid UserId { get; init; } = userId;
    public Guid AssignmentId { get; set; } = assignmentId;

    private static ushort CalculateFinalScore(ushort compilationScore, ushort qualityScore, ushort testsScore)
    {
        return (ushort)(compilationScore + qualityScore + testsScore);
    }

    public static Attempt Create(ushort position, Guid userId, Guid assignmentId)
    {
        return new Attempt(
            Guid.NewGuid(),
            position,
            null,
            false,
            DateTime.UtcNow,
            DateTime.UtcNow,
            userId,
            assignmentId
        );
    }

    public void UpdateBranchName(string? branchName)
    {
        BranchName = branchName;
        UpdatedAt = DateTime.UtcNow;
    }

    private void UpdateScores(ushort compilationScore, ushort qualityScore, ushort testsScore)
    {
        CompilationScore = compilationScore;
        QualityScore = qualityScore;
        TestsScore = testsScore;
        FinalScore = CalculateFinalScore(compilationScore, qualityScore, testsScore);
        UpdatedAt = DateTime.UtcNow;
    }

    public void Submit(string branchName, ushort compilationScore, ushort qualityScore, ushort testsScore)
    {
        BranchName = branchName;
        UpdateScores(compilationScore, qualityScore, testsScore);
        IsCompleted = true;
    }
}