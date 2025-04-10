namespace HomeAssignment.DTOs.RespondDTOs.CourseRelated;

public class RespondEnrollmentDto
{
    public Guid Id { get; init; }
    public required Guid UserId { get; set; }
    public required Guid CourseId { get; set; }
}