using HomeAssignment.DTOs.RespondDTOs.AssignmentRelated;

namespace HomeAssignment.DTOs.RespondDTOs.ChapterRelated;

public class RespondChapterWithAssignmentsDto
{
    public Guid Id { get; init; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public string? VideoUrl { get; set; }

    public int Position { get; set; }

    public bool IsPublished { get; set; }
    public bool IsFree { get; set; }

    public Guid? CourseId { get; set; }
    public required List<RespondAssignmentDto> Assignments { get; set; }
    public List<RespondAttachmentDto>? Attachments { get; set; }
}