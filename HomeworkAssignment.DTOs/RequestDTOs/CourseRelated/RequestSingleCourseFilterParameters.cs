namespace HomeAssignment.DTOs.RequestDTOs;

public class RequestSingleCourseFilterParameters
{
    public required Guid OwnerId { get; set; }
    public string[]? Include { get; set; }
}