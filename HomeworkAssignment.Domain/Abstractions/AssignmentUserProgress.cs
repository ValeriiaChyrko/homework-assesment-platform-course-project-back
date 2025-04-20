namespace HomeAssignment.Domain.Abstractions;

public class AssignmentUserProgress(
    Guid id,
    bool isCompleted,
    Guid userId,
    Guid? assignmentId,
    DateTime createdAt,
    DateTime updatedAt)
{
    public Guid Id { get; init; } = id;

    public bool IsCompleted { get; set; } = isCompleted;

    public Guid UserId { get; init; } = userId;
    public Guid? AssignmentId { get; init; } = assignmentId;

    public DateTime CreatedAt { get; init; } = createdAt;
    public DateTime UpdatedAt { get; private set; } = updatedAt;

    public static AssignmentUserProgress Create(bool isCompleted, Guid userId, Guid assignmentId)
    {
        return new AssignmentUserProgress(
            Guid.NewGuid(),
            isCompleted,
            userId,
            assignmentId,
            DateTime.UtcNow,
            DateTime.UtcNow
        );
    }

    public void Update(bool isCompleted)
    {
        IsCompleted = isCompleted;
        UpdatedAt = DateTime.UtcNow;
    }
}