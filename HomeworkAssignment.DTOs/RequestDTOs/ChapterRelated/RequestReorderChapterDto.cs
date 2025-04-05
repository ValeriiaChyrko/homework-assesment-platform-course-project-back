namespace HomeAssignment.DTOs.RequestDTOs;

public class RequestReorderChapterDto
{
    public required Guid Id { get; set; }
    public int Position { get; set; }
}