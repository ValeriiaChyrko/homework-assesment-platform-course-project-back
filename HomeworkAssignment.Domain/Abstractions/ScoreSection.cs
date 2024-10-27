namespace HomeAssignment.Domain.Abstractions;

public class ScoreSection
{
    public bool IsEnabled { get; set; }
    public int MaxScore { get; set; }
    public int MinScore { get; set; }

    public ScoreSection(bool isEnabled, int maxScore, int minScore)
    {
        IsEnabled = isEnabled;
        MaxScore = maxScore;
        MinScore = minScore;
    }
}