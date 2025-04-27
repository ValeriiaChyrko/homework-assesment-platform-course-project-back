namespace HomeAssignment.Domain.Abstractions;

public class Enrollment(Guid id, Guid userId, Guid courseId, DateTime createdAt, DateTime updatedAt)
{
    public Guid Id { get; init; } = id;

    public Guid UserId { get; init; } = userId;
    public Guid CourseId { get; init; } = courseId;

    public DateTime CreatedAt { get; init; } = createdAt;
    public DateTime UpdatedAt { get; init; } = updatedAt;

    public static Enrollment Create(Guid userId, Guid courseId)
    {
        return new Enrollment(
            Guid.NewGuid(),
            userId,
            courseId,
            DateTime.UtcNow,
            DateTime.UtcNow
        );
    }
}