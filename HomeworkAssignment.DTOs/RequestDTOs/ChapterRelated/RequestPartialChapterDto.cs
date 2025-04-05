namespace HomeAssignment.DTOs.RequestDTOs;

public class RequestPartialChapterDto
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? VideoUrl { get; set; }
    public int Position { get; set; }
    public bool? IsFree { get; set; }
}