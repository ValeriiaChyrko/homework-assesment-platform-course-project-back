namespace HomeAssignment.DTOs.RequestDTOs.CourseRelated;

public class RequestPartialCourseDto
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public Guid? CategoryId { get; set; }
}