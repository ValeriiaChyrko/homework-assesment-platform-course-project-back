using HomeAssignment.DTOs.SharedDTOs;

namespace HomeAssignment.DTOs.RequestDTOs;

public class RequestAssignmentDto
{
    public Guid OwnerGitHubAccountId { get; init; }
    public required string Title { get; init; }
    public required string RepositoryName { get; init; }
    public string? Description { get; init; }
    public DateTime Deadline { get; init; }
    public int MaxScore { get; init; }
    public int MaxAttemptsAmount { get; init; }

    public ScoreSectionDto? CompilationSection { get; init; }
    public ScoreSectionDto? TestsSection { get; init; }
    public ScoreSectionDto? QualitySection { get; init; }
}