namespace HomeAssignment.Domain.Abstractions;

public class Attachment
{
    public Attachment(Guid id, string name, string url, Guid? courseId, Guid? chapterId, DateTime createdAt)
    {
        if (courseId != null && chapterId != null)
        {
            throw new ArgumentException("Attachment cannot belong to both a course and a chapter.");
        }

        if (courseId == null && chapterId == null)
        {
            throw new ArgumentException("Attachment must belong to either a course or a chapter.");
        }

        Id = id;
        Name = name;
        Url = url;
        CourseId = courseId;
        ChapterId = chapterId;
        CreatedAt = createdAt;
        UpdatedAt = createdAt;
    }

    public Guid Id { get; init; }
    public string Name { get; set; }
    public string Url { get; set; }

    public Guid? CourseId { get; set; }
    public Guid? ChapterId { get; set; }

    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; set; }

    public static Attachment CreateForCourse(Guid courseId, string name, string url)
    {
        return new Attachment(Guid.NewGuid(), name, url, courseId, null, DateTime.UtcNow);
    }

    public static Attachment CreateForChapter(Guid chapterId, string name, string url)
    {
        return new Attachment(Guid.NewGuid(), name, url, null, chapterId, DateTime.UtcNow);
    }

    public void Update(string name, string url)
    {
        Name = name;
        Url = url;
        UpdatedAt = DateTime.UtcNow;
    }
}