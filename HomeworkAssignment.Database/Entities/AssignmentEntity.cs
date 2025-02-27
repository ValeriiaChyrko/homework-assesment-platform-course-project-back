namespace HomeAssignment.Database.Entities;

public sealed class AssignmentEntity
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public string? RepositoryName { get; set; }
    public string? RepositoryOwner { get; set; }
    public string? RepositoryUrl { get; set; }
    public DateTime Deadline { get; set; } = DateTime.Now;

    public int MaxScore { get; set; } = 100;
    public int MaxAttemptsAmount { get; set; } = 5;

    public int Position { get; set; }
    public bool IsPublished { get; set; } = false;

    public bool AttemptCompilationSectionEnable { get; set; } = false;
    public bool AttemptTestsSectionEnable { get; set; } = false;
    public bool AttemptQualitySectionEnable { get; set; } = false;

    public int AttemptCompilationMaxScore { get; set; } = 5;
    public int AttemptCompilationMinScore { get; set; } = 0;

    public int AttemptTestsMaxScore { get; set; } = 65;
    public int AttemptTestsMinScore { get; set; } = 0;

    public int AttemptQualityMaxScore { get; set; } = 30;
    public int AttemptQualityMinScore { get; set; } = 0;
    
    public DateTime CreatedAt { get; set; } 
    public DateTime UpdatedAt { get; set; }

    public Guid ChapterId { get; set; }
    public ChapterEntity? Chapter { get; set; }

    public ICollection<AttemptProgressEntity>? Attempts { get; set; } 
}