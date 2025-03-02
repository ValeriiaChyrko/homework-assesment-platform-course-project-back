namespace HomeAssignment.Domain.Abstractions;

public class Assignment
{
    private readonly List<Guid> _attemptIds;
    
    public Assignment(Guid id, string title, string? description, string? repositoryName, string? repositoryOwnerUserName, 
        string? repositoryUrl, DateTime deadline, int maxScore, int maxAttemptsAmount, int position, bool isPublished, 
        Guid? chapterId, List<Guid>? attemptIds, DateTime createdAt, DateTime updatedAt, 
        ScoreSection compilationSection, ScoreSection testsSection, ScoreSection qualitySection)
    {
        Id = id;
        Title = title;
        Description = description;
        RepositoryName = repositoryName;
        RepositoryOwnerUserName = repositoryOwnerUserName;
        RepositoryUrl = repositoryUrl;
        Deadline = deadline;
        MaxScore = maxScore;
        MaxAttemptsAmount = maxAttemptsAmount;
        Position = position;
        IsPublished = isPublished;
        ChapterId = chapterId;
        _attemptIds = attemptIds ?? [];
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        CompilationSection = compilationSection;
        TestsSection = testsSection;
        QualitySection = qualitySection;
    }

    public Guid Id { get; init; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public string? RepositoryName { get; set; }
    public string? RepositoryOwnerUserName { get; set; }
    public string? RepositoryUrl { get; set; }
    public DateTime Deadline { get; set; } 

    public int MaxScore { get; set; }
    public int MaxAttemptsAmount { get; set; } 

    public int Position { get; set; }
    public bool IsPublished { get; set; } 
    
    public Guid? ChapterId { get; set; }
    
    public IReadOnlyList<Guid> AttemptProgressIds => _attemptIds.AsReadOnly();
    
    public DateTime CreatedAt { get; init; } 
    public DateTime UpdatedAt { get; set; }

    public ScoreSection CompilationSection { get; set; }
    public ScoreSection TestsSection { get; set; }
    public ScoreSection QualitySection { get; set; }

    public static Assignment Create(string title, string? description, string? repositoryName, string? repositoryOwner, 
        string? repositoryUrl, DateTime deadline, int maxScore, int maxAttemptsAmount, int position, bool isPublished, 
        Guid? chapterId, List<Guid>? attemptIds,
        ScoreSection? compilationSection = null, ScoreSection? testsSection = null, ScoreSection? qualitySection = null)
    {
        return new Assignment(
            Guid.NewGuid(),
            title,
            description,
            repositoryName,
            repositoryOwner,
            repositoryUrl,
            deadline,
            maxScore,
            maxAttemptsAmount,
            position,
            isPublished,
            chapterId,
            attemptIds ?? [],
            DateTime.UtcNow,
            DateTime.UtcNow,
            compilationSection ?? new ScoreSection(false, 0, 0),
            testsSection ?? new ScoreSection(false, 0, 0),
            qualitySection ?? new ScoreSection(false, 0, 0)
        );
    }

    public void Update(string title, string? description, string? repositoryName, string? repositoryOwner, 
    string? repositoryUrl, DateTime deadline, int maxScore, int maxAttemptsAmount, int position, 
        ScoreSection? compilationSection = null, ScoreSection? testsSection = null, ScoreSection? qualitySection = null)
    {
        Title = title;
        Description = description;
        RepositoryName = repositoryName;
        RepositoryOwnerUserName = repositoryOwner;
        RepositoryUrl = repositoryUrl;
        Deadline = deadline;
        MaxScore = maxScore;
        MaxAttemptsAmount = maxAttemptsAmount;
        Position = position;
        
        UpdatedAt = DateTime.UtcNow;
        
        CompilationSection = compilationSection ?? new ScoreSection(false, 0, 0);
        TestsSection = testsSection ?? new ScoreSection(false, 0, 0);
        QualitySection = qualitySection ?? new ScoreSection(false, 0, 0);
    }

    public void Publish()
    {
        UpdatedAt = DateTime.UtcNow;
        
        IsPublished = true;
    }
    
    public void Unpublish()
    {
        UpdatedAt = DateTime.UtcNow;
        
        IsPublished = false;
    }
    
    public void AddAttempt(Guid attemptId)
    {
        if (_attemptIds?.Count >= MaxAttemptsAmount)
        {
            throw new InvalidOperationException("Maximum number of attempts reached.");
        }

        _attemptIds?.Add(attemptId);
        UpdatedAt = DateTime.UtcNow;
    }

    public void RemoveAttempt(Guid attemptId)
    {
        if (_attemptIds!.Remove(attemptId))
        {
            UpdatedAt = DateTime.UtcNow;
        }
    }
}