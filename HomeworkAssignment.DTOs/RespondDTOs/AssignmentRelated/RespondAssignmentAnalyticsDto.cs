using HomeAssignment.DTOs.RespondDTOs.AttemptRelated;

namespace HomeAssignment.DTOs.RespondDTOs.AssignmentRelated;

public class RespondAssignmentAnalyticsDto
{
    public List<RespondAttemptAnalyticsDto> Attempts { get; set; } = [];
    public required string AssignmentTitle { get; set; }

    public double AverageScore { get; set; }
    public ushort HighestScore { get; set; }
    public ushort SuccessCoefficient { get; set; }
}