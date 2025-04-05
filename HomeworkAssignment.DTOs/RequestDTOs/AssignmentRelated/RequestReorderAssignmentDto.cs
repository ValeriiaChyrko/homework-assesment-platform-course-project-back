namespace HomeAssignment.DTOs.RequestDTOs;

public class RequestReorderAssignmentDto
{
    public required Guid Id { get; set; }
    public ushort Position { get; set; }
}