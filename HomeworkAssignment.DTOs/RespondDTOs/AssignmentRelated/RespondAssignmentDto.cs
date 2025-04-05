namespace HomeAssignment.DTOs.RespondDTOs;

public class RespondAssignmentDto
{
    public Guid Id { get; init; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public string? RepositoryName { get; set; }
    public string? RepositoryOwner { get; set; }
    public string? RepositoryUrl { get; set; }
    public DateTime Deadline { get; set; } 

    public int MaxScore { get; set; } 
    public int MaxAttemptsAmount { get; set; } 
    public int Position { get; set; }
    
    public bool IsPublished { get; set; } 
    
    public Guid? ChapterId { get; set; }

    public bool AttemptCompilationSectionEnable { get; set; }
    public bool AttemptTestsSectionEnable { get; set; }
    public bool AttemptQualitySectionEnable { get; set; }

    public int AttemptCompilationMaxScore { get; set; }
    public int AttemptCompilationMinScore { get; set; }

    public int AttemptTestsMaxScore { get; set; } 
    public int AttemptTestsMinScore { get; set; }

    public int AttemptQualityMaxScore { get; set; } 
    public int AttemptQualityMinScore { get; set; } 
}