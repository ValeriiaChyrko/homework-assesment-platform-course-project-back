namespace HomeAssignment.Domain.Abstractions
{
    public class Assignment(
        Guid id,
        string title,
        string? description,
        string? repositoryName,
        string? repositoryOwnerUserName,
        string? repositoryUrl,
        DateTime deadline,
        ushort maxScore,
        ushort maxAttemptsAmount,
        ushort position,
        bool isPublished,
        Guid? chapterId,
        List<Guid>? attemptIds,
        DateTime createdAt,
        DateTime updatedAt,
        ScoreSection compilationSection,
        ScoreSection testsSection,
        ScoreSection qualitySection)
    {
        private readonly List<Guid> _attemptIds = attemptIds ?? [];

        public Guid Id { get; init; } = id;
        public string Title { get; set; } = title;
        public string? Description { get; set; } = description;
        public string? RepositoryName { get; set; } = repositoryName;
        public string? RepositoryOwnerUserName { get; set; } = repositoryOwnerUserName;
        public string? RepositoryUrl { get; set; } = repositoryUrl;
        public DateTime Deadline { get; set; } = deadline;
        public ushort MaxScore { get; set; } = maxScore;
        public ushort MaxAttemptsAmount { get; set; } = maxAttemptsAmount;
        public ushort Position { get; set; } = position;
        public bool IsPublished { get; private set; } = isPublished;
        public Guid? ChapterId { get; set; } = chapterId;
        public IReadOnlyList<Guid> AttemptProgressIds => _attemptIds.AsReadOnly();
        public DateTime CreatedAt { get; init; } = createdAt;
        public DateTime UpdatedAt { get; private set; } = updatedAt;
        public ScoreSection CompilationSection { get; set; } = compilationSection;
        public ScoreSection TestsSection { get; set; } = testsSection;
        public ScoreSection QualitySection { get; set; } = qualitySection;

        public static Assignment Create(string title)
        {
            return new Assignment(
                Guid.NewGuid(),
                title,
                null,
                null,
                null,
                null,
                DateTime.UtcNow,
                0,
                0,
                0,
                false,
                null,
                [],
                DateTime.UtcNow,
                DateTime.UtcNow,
                new ScoreSection(false, 0, 0),
                new ScoreSection(false, 0, 0),
                new ScoreSection(false, 0, 0)
            );
        }

        public void PatchUpdate(
            string? title = null,
            string? description = null,
            string? repositoryName = null,
            string? repositoryOwner = null,
            string? repositoryUrl = null,
            DateTime? deadline = null,
            ushort? maxScore = null,
            ushort? maxAttemptsAmount = null,
            ushort? position = null,
            ScoreSection? compilationSection = null,
            ScoreSection? testsSection = null,
            ScoreSection? qualitySection = null)
        {
            Title = title ?? Title;
            Description = description ?? Description;
            RepositoryName = repositoryName ?? RepositoryName;
            RepositoryOwnerUserName = repositoryOwner ?? RepositoryOwnerUserName;
            RepositoryUrl = repositoryUrl ?? RepositoryUrl;
            Deadline = deadline ?? Deadline;
            MaxScore = maxScore ?? MaxScore;
            MaxAttemptsAmount = maxAttemptsAmount ?? MaxAttemptsAmount;
            Position = position ?? Position;
            CompilationSection = compilationSection ?? CompilationSection;
            TestsSection = testsSection ?? TestsSection;
            QualitySection = qualitySection ?? QualitySection;

            UpdatedAt = DateTime.UtcNow;
        }

        public void Publish()
        {
            IsPublished = true;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Unpublish()
        {
            IsPublished = false;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
