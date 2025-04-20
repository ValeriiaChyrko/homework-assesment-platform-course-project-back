namespace HomeAssignment.Domain.Abstractions;

public class Attachment
{
    public Attachment(Guid id, string uploadthingKey, string name, string url, Guid? courseId, Guid? chapterId,
        DateTime createdAt)
    {
        if (courseId != null && chapterId != null)
            throw new ArgumentException("Attachment cannot belong to both a course and a chapter.");

        if (courseId == null && chapterId == null)
            throw new ArgumentException("Attachment must belong to either a course or a chapter.");

        Id = id;
        UploadthingKey = uploadthingKey;
        Name = name;
        Url = url;
        CourseId = courseId;
        ChapterId = chapterId;
        CreatedAt = createdAt;
        UpdatedAt = createdAt;
    }

    public Guid Id { get; init; }
    public string UploadthingKey { get; private set; }
    public string Name { get; private set; }
    public string Url { get; private set; }

    public Guid? CourseId { get; init; }
    public Guid? ChapterId { get; init; }

    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; private set; }

    public static Attachment CreateForCourse(Guid courseId, string uploadthingKey, string name, string url)
    {
        return new Attachment(Guid.NewGuid(), uploadthingKey, name, url, courseId, null, DateTime.UtcNow);
    }

    public static Attachment CreateForChapter(Guid chapterId, string uploadthingKey, string name, string url)
    {
        return new Attachment(Guid.NewGuid(), uploadthingKey, name, url, null, chapterId, DateTime.UtcNow);
    }
}