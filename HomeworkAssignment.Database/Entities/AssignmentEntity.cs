namespace HomeAssignment.Database.Entities;

public sealed class AssignmentEntity
{
    public Guid Id { get; set; }
    public required string Title { get; init; }
    public string? Description { get; init; }
    public string? RepositoryName { get; init; }
    public string? RepositoryBaseBranchName { get; init; }
    public string? RepositoryOwner { get; init; }
    public string? RepositoryUrl { get; init; }
    public DateTime? Deadline { get; init; }

    public ushort MaxScore { get; init; }
    public ushort MaxAttemptsAmount { get; init; }

    public ushort Position { get; set; }
    public bool IsPublished { get; init; }

    public bool AttemptCompilationSectionEnable { get; init; }
    public bool AttemptTestsSectionEnable { get; init; }
    public bool AttemptQualitySectionEnable { get; init; }

    public ushort AttemptCompilationMaxScore { get; init; }
    public ushort AttemptCompilationMinScore { get; init; }

    public ushort AttemptTestsMaxScore { get; init; }
    public ushort AttemptTestsMinScore { get; init; }

    public ushort AttemptQualityMaxScore { get; init; }
    public ushort AttemptQualityMinScore { get; init; }

    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; set; }


    public Guid? ChapterId { get; init; }
    public ChapterEntity? Chapter { get; init; }


    public ICollection<UserAssignmentProgressEntity>? UsersProgress { get; init; }
    public ICollection<AttemptEntity>? Attempts { get; init; }
}