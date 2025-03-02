namespace HomeAssignment.DTOs.RequestDTOs;

public class RequestAttachmentDto
{
    public required string Name { get; set; }
    public required string Url { get; set; }

    public Guid? CourseId { get; set; }
    public Guid? ChapterId { get; set; }
}