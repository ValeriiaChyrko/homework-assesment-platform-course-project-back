namespace HomeAssignment.DTOs.RequestDTOs;

public class RequestReorderChapterDto
{
    public required Guid Id { get; set; }
    public ushort Position { get; set; }
}