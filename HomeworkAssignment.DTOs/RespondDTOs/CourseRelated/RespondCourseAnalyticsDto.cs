namespace HomeAssignment.DTOs.RespondDTOs.CourseRelated;

public class RespondCourseAnalyticsDto
{
    public required Guid CourseId { get; set; }
    public required string CourseTitle { get; set; }
    public int EnrollmentsAmount { get; set; }
}