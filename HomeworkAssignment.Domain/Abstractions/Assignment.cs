namespace HomeAssignment.Domain.Abstractions;

public class Assignment
{
    public Assignment(Guid id, Guid ownerId, string title, string? description, DateTime deadline, int maxScore,
        int maxAttemptsAmount,
        ScoreSection compilationSection, ScoreSection testsSection, ScoreSection qualitySection)
    {
        Id = id;
        OwnerId = ownerId;
        Title = title;
        Description = description;
        Deadline = deadline;
        MaxScore = maxScore;
        MaxAttemptsAmount = maxAttemptsAmount;
        CompilationSection = compilationSection;
        TestsSection = testsSection;
        QualitySection = qualitySection;
    }

    public Guid Id { get; set; }
    public Guid OwnerId { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public DateTime Deadline { get; set; }
    public int MaxScore { get; set; }
    public int MaxAttemptsAmount { get; set; }

    public ScoreSection CompilationSection { get; set; }
    public ScoreSection TestsSection { get; set; }
    public ScoreSection QualitySection { get; set; }

    public static Assignment Create(Guid ownerId, string title, string? description, DateTime deadline, int maxScore,
        int maxAttemptsAmount = 1, ScoreSection? compilationSection = null, ScoreSection? testsSection = null,
        ScoreSection? qualitySection = null)
    {
        var assignmentId = Guid.NewGuid();

        return new Assignment(
            assignmentId,
            ownerId,
            title,
            description,
            deadline,
            maxScore,
            maxAttemptsAmount,
            compilationSection ?? new ScoreSection(false, 0, 0),
            testsSection ?? new ScoreSection(false, 0, 0),
            qualitySection ?? new ScoreSection(false, 0, 0)
        );
    }

    public void Update(Guid ownerId, string title, string? description, DateTime deadline, int maxScore,
        int maxAttemptsAmount = 1, ScoreSection? compilationSection = null, ScoreSection? testsSection = null,
        ScoreSection? qualitySection = null)
    {
        OwnerId = ownerId;
        Title = title;
        Description = description;
        Deadline = deadline;
        MaxScore = maxScore;
        MaxAttemptsAmount = maxAttemptsAmount;
        CompilationSection = compilationSection ?? new ScoreSection(false, 0, 0);
        TestsSection = testsSection ?? new ScoreSection(false, 0, 0);
        QualitySection = qualitySection ?? new ScoreSection(false, 0, 0);
    }
}