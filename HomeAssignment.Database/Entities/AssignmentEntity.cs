namespace HomeAssignment.Database.Entities;

public sealed class AssignmentEntity
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public DateTime Deadline { get; set; }
    public int MaxScore { get; set; }
    public int MaxAttemptsAmount { get; set; }
    
    public bool AttemptCompilationSectionEnable { get; set; }
    public bool AttemptTestsSectionEnable { get; set; }
    public bool AttemptQualitySectionEnable { get; set; }
    
    public int AttemptCompilationMaxScore { get; set; }
    public int AttemptCompilationMinScore { get; set; }
    
    public int AttemptTestsMaxScore { get; set; }
    public int AttemptTestsMinScore { get; set; }
    
    public int AttemptQualityMaxScore { get; set; }
    public int AttemptQualityMinScore { get; set; }
    
    public Guid OwnerId  { get; set; }
    public GitHubProfilesEntity OwnerEntity { get; set; } = null!;
    
    public ICollection<AttemptEntity>? Attempts { get; set; }
}