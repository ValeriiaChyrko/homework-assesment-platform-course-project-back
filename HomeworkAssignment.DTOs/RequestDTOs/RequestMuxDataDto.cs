namespace HomeAssignment.DTOs.RequestDTOs;

public class RequestMuxDataDto
{
    public required string AssetId { get; set; }
    public string? PlaybackId { get; set; }
    
    public Guid? ChapterId { get; set; }
}