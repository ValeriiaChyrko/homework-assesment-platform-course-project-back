namespace HomeAssignment.DTOs.RespondDTOs.CourseRelated;

public class RespondEnrollmentWithCourseDto
{
    public Guid Id { get; init; }
    public required Guid UserId { get; set; }
    public required RespondCourseDto Course { get; set; }
}