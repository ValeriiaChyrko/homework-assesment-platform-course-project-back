namespace HomeAssignment.DTOs.RespondDTOs;

public class RespondAttachmentDto
{
    public Guid Id { get; init; }
    public required string Key { get; set; }
    public required string Name { get; set; }
    public required string Url { get; set; }

    public Guid? CourseId { get; set; }
    public Guid? ChapterId { get; set; }
}