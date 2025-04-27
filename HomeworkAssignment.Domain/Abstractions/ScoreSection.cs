namespace HomeAssignment.Domain.Abstractions;

public class ScoreSection(bool isEnabled, ushort maxScore, ushort minScore)
{
    public bool IsEnabled { get; set; } = isEnabled;
    public ushort MaxScore { get; set; } = maxScore;
    public ushort MinScore { get; set; } = minScore;
}