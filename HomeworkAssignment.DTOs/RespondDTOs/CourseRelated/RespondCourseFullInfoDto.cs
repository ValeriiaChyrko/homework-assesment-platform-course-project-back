using HomeAssignment.DTOs.RespondDTOs.CategoryRelated;
using HomeAssignment.DTOs.RespondDTOs.ChapterRelated;

namespace HomeAssignment.DTOs.RespondDTOs.CourseRelated;

public class RespondCourseFullInfoDto
{
    public Guid Id { get; init; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required string ImageUrl { get; set; }

    public bool IsEnrolled { get; set; }
    public bool IsPublished { get; set; }

    public int Progress { get; set; }

    public RespondCategoryDto? Category { get; set; }
    public required List<RespondChapterDto>? Chapters { get; set; }
    public required List<RespondAttachmentDto>? Attachments { get; set; }
}