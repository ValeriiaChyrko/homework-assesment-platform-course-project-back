namespace HomeAssignment.DTOs.RespondDTOs.CourseRelated;

public class RespondEnrollmentsAnalyticsDto
{
    public required List<RespondCourseAnalyticsDto> Analysis { get; set; }
    public required int TotalStudents { get; set; }
}