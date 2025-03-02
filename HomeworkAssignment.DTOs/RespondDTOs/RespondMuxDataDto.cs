namespace HomeAssignment.DTOs.RespondDTOs;

public class RespondMuxDataDto
{
    public Guid Id { get; init; }
    public required string AssetId { get; set; }
    public string? PlaybackId { get; set; }
    
    public Guid? ChapterId { get; set; }
}