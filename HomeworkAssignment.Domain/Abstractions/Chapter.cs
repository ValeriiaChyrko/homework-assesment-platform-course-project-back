namespace HomeAssignment.Domain.Abstractions
{
    public class Chapter(
        Guid id,
        string title,
        string? description,
        string? videoUrl,
        ushort position,
        bool isPublished,
        bool isFree,
        Guid? courseId,
        List<Guid>? attachmentIds,
        List<Guid>? userProgressIds,
        DateTime createdAt,
        DateTime updatedAt)
    {
        private readonly List<Guid> _attachmentIds = attachmentIds ?? [];
        private readonly List<Guid> _userProgressIds = userProgressIds ?? [];

        public Guid Id { get; init; } = id;
        public string Title { get; set; } = title;
        public string? Description { get; set; } = description;
        public string? VideoUrl { get; set; } = videoUrl;
        public int Position { get; set; } = position;
        public bool IsPublished { get; private set; } = isPublished;
        public bool IsFree { get; set; } = isFree;
        public Guid? CourseId { get; set; } = courseId;
        public IReadOnlyList<Guid> AttachmentIds => _attachmentIds.AsReadOnly();
        public IReadOnlyList<Guid> UserProgressIds => _userProgressIds.AsReadOnly();
        public DateTime CreatedAt { get; init; } = createdAt;
        public DateTime UpdatedAt { get; private set; } = updatedAt;

        public static Chapter Create(string title)
        {
            return new Chapter(
                Guid.NewGuid(),
                title,
                null,
                null,
                0,
                false,
                true,
                null,
                new List<Guid>(),
                new List<Guid>(),
                DateTime.UtcNow,
                DateTime.UtcNow
            );
        }
        
        public void PatchUpdate(string? title = null, string? description = null, string? videoUrl = null,
            int? position = null, bool? isFree = null)
        {
            Title = title ?? Title;
            Description = description ?? Description;
            VideoUrl = videoUrl ?? VideoUrl;
            Position = position ?? Position;
            IsFree = isFree ?? IsFree;

            UpdatedAt = DateTime.UtcNow;
        }
        
        public void MarkAsPublished()
        {
            IsPublished = true;
            UpdatedAt = DateTime.UtcNow;
        }
        
        public void MarkAsUnpublished()
        {
            IsPublished = false;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}