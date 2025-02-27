namespace HomeAssignment.Database.Entities;

public class MuxDataEntity
{
    public Guid Id { get; set; }
    public required string AssetId { get; set; }
    public string? PlaybackId { get; set; }
    
    public Guid? ChapterId { get; set; }
    public ChapterEntity? Chapter { get; set; }
}