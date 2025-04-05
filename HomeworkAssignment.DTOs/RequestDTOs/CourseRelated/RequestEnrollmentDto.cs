namespace HomeAssignment.DTOs.RequestDTOs;

public class RequestEnrollmentDto
{
    public required Guid UserId { get; set; }
    public required Guid CourseId { get; set; }
}