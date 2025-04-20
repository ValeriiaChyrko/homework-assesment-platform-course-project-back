namespace HomeAssignment.DTOs.RespondDTOs.ChapterRelated;

public class RespondChapterUserProgressDto
{
    public Guid Id { get; init; }
    public bool IsCompleted { get; set; }

    public Guid UserId { get; set; }
    public Guid ChapterId { get; set; }
}