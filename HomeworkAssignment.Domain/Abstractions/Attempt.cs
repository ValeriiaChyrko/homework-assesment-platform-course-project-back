using HomeAssignment.Domain.Abstractions.Enums;

namespace HomeAssignment.Domain.Abstractions;

public class Attempt
{
    public Attempt(Guid id, int position, string? branchName, int finalScore, int compilationScore, int qualityScore, int testsScore, string progressStatus, bool isCompleted, DateTime createdAt, DateTime updatedAt, Guid userId, Guid assignmentId)
    {
        Id = id;
        Position = position;
        BranchName = branchName;
        FinalScore = finalScore;
        CompilationScore = compilationScore;
        QualityScore = qualityScore;
        TestsScore = testsScore;
        ProgressStatus = progressStatus;
        IsCompleted = isCompleted;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        UserId = userId;
        AssignmentId = assignmentId;
    }

    public Guid Id { get; set; } 
    public int Position { get; set; }

    public string? BranchName { get; set; }
    
    public int FinalScore { get; set; } = 0;
    
    public int CompilationScore { get; set; } = 0;
    public int QualityScore { get; set; } = 0;
    public int TestsScore { get; set; } = 0;

    public string ProgressStatus { get; set; }
    public bool IsCompleted { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    public Guid UserId { get; set; }
    public Guid AssignmentId { get; set; }

    public static Attempt Create(int position, string? branchName, int finalScore, int compilationScore,
        int qualityScore, int testsScore, bool isCompleted, Guid userId, Guid assignmentId)
    {
        if (compilationScore + qualityScore + testsScore != finalScore)
        {
            throw new ArgumentException($"The sum of scores (compilation: {compilationScore}, quality: {qualityScore}, tests: {testsScore}) does not equal the final score ({finalScore}).");
        }
        
        return new Attempt(
            Guid.NewGuid(), 
            position,
            branchName,
            finalScore,
            compilationScore,
            qualityScore,
            testsScore,
            ProgressStatuses.Started.ToString().ToLower(),
            isCompleted,
            DateTime.UtcNow, 
            DateTime.UtcNow,
            userId,
            assignmentId
        );
    }

    public void Update(int position, string? branchName, int finalScore, int compilationScore,
        int qualityScore, int testsScore, Guid userId, Guid assignmentId)
    {
        Position = position;
        BranchName = branchName;
        FinalScore = finalScore;
        CompilationScore = compilationScore;
        QualityScore = qualityScore;
        TestsScore = testsScore;
        UpdatedAt = DateTime.UtcNow;
        UserId = userId;
        AssignmentId = assignmentId;
    }

    public void Start()
    {
        UpdatedAt = DateTime.UtcNow;
        IsCompleted = false;
        ProgressStatus = ProgressStatuses.InProgress.ToString().ToLower();
    }

    public void Submit()
    {
        UpdatedAt = DateTime.UtcNow;
        IsCompleted = false;
        ProgressStatus = ProgressStatuses.Submitted.ToString().ToLower();
    }
    
    public void Complete()
    {
        UpdatedAt = DateTime.UtcNow;
        IsCompleted = true;
        ProgressStatus = ProgressStatuses.Finished.ToString().ToLower();
    }
}