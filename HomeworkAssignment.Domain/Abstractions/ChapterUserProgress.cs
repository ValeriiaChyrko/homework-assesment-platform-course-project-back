namespace HomeAssignment.Domain.Abstractions;

public class ChapterUserProgress(
    Guid id,
    bool isCompleted,
    Guid userId,
    Guid? chapterId,
    DateTime createdAt,
    DateTime updatedAt)
{
    public Guid Id { get; init; } = id;

    public bool IsCompleted { get; private set; } = isCompleted;

    public Guid UserId { get; init; } = userId;
    public Guid? ChapterId { get; init; } = chapterId;

    public DateTime CreatedAt { get; init; } = createdAt;
    public DateTime UpdatedAt { get; private set; } = updatedAt;

    public static ChapterUserProgress CreateForChapter(bool isCompleted, Guid userId, Guid chapterId)
    {
        if (userId == Guid.Empty) throw new ArgumentException("User ID must not be empty.");
        if (chapterId == Guid.Empty) throw new ArgumentException("Chapter ID must not be empty.");

        return new ChapterUserProgress(
            Guid.NewGuid(),
            isCompleted,
            userId,
            chapterId,
            DateTime.UtcNow,
            DateTime.UtcNow
        );
    }

    public void UpdateProgress(bool isCompleted)
    {
        IsCompleted = isCompleted;
        UpdatedAt = DateTime.UtcNow;
    }
}