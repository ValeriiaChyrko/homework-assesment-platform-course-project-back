using HomeAssignment.DTOs.SharedDTOs;

namespace HomeAssignment.DTOs.RespondDTOs;

public class RespondAssignmentDto
{
    public Guid Id { get; set; }
    public Guid OwnerId { get; init; }
    public string Title { get; init; } = string.Empty;
    public string? Description { get; init; }
    public DateTime Deadline { get; init; }
    public int MaxScore { get; init; }
    public int MaxAttemptsAmount { get; init; }

    public ScoreSectionDto? CompilationSection { get; init; }
    public ScoreSectionDto? TestsSection { get; init; }
    public ScoreSectionDto? QualitySection { get; init; }
}