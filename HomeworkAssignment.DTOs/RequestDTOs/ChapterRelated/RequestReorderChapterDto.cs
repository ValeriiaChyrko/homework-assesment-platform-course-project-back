namespace HomeAssignment.DTOs.RequestDTOs.ChapterRelated;

public class RequestReorderChapterDto
{
    public required Guid Id { get; set; }
    public ushort Position { get; set; }
}