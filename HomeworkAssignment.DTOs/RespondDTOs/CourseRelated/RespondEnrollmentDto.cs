namespace HomeAssignment.DTOs.RespondDTOs;

public class RespondEnrollmentDto
{
    public Guid Id { get; init; }
    public required Guid UserId { get; set; }
    public required Guid CourseId { get; set; }
}