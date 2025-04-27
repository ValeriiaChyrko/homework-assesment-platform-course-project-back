namespace HomeAssignment.DTOs.RespondDTOs.AssignmentRelated;

public class RespondAssignmentDto
{
    public Guid Id { get; init; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public string? RepositoryName { get; set; }
    public string? RepositoryBaseBranchName { get; set; }
    public string? RepositoryOwner { get; set; }
    public string? RepositoryUrl { get; set; }
    public DateTime Deadline { get; set; }

    public ushort MaxScore { get; set; }
    public ushort MaxAttemptsAmount { get; set; }
    public ushort Position { get; set; }

    public bool IsPublished { get; set; }

    public Guid? ChapterId { get; set; }

    public bool AttemptCompilationSectionEnable { get; set; }
    public bool AttemptTestsSectionEnable { get; set; }
    public bool AttemptQualitySectionEnable { get; set; }

    public ushort AttemptCompilationMaxScore { get; set; }
    public ushort AttemptCompilationMinScore { get; set; }

    public ushort AttemptTestsMaxScore { get; set; }
    public ushort AttemptTestsMinScore { get; set; }

    public ushort AttemptQualityMaxScore { get; set; }
    public ushort AttemptQualityMinScore { get; set; }
}