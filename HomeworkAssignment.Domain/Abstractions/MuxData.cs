namespace HomeAssignment.Domain.Abstractions;

public class MuxData
{
    public MuxData(Guid id, string assetId, string? playbackId, Guid? chapterId)
    {
        Id = id;
        AssetId = assetId;
        PlaybackId = playbackId;
        ChapterId = chapterId;
    }

    public Guid Id { get; init; }
    public string AssetId { get; set; }
    public string? PlaybackId { get; set; }
    
    public Guid? ChapterId { get; init; }

    public static MuxData Create(string assetId, string? playbackId, Guid? chapterId)
    {
        return new MuxData(
            Guid.NewGuid(),
            assetId,
            playbackId,
            chapterId
        );
    }

    public void Update(string assetId, string? playbackId)
    {
        AssetId = assetId;
        PlaybackId = playbackId;
    }
}