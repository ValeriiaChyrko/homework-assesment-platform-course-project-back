namespace HomeAssignment.DTOs.RespondDTOs.AttemptRelated;

public class RespondAttemptAnalyticsDto
{
    public ushort FinalScore { get; set; }

    public ushort CompilationScore { get; set; }
    public ushort QualityScore { get; set; }
    public ushort TestsScore { get; set; }
    
    public required string? StudentFullName { get; set; }
}