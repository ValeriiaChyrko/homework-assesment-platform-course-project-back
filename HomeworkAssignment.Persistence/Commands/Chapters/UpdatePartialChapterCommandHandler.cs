using HomeAssignment.Database.Contexts.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Commands.Chapters;

public sealed record UpdatePartialChapterCommandHandler : IRequestHandler<UpdatePartialChapterCommand>
{
    private readonly IHomeworkAssignmentDbContext _context;

    public UpdatePartialChapterCommandHandler(IHomeworkAssignmentDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task Handle(UpdatePartialChapterCommand command,
        CancellationToken cancellationToken = default)
    {
        if (command is null) throw new ArgumentNullException(nameof(command));

        var chapterEntity = await _context.ChapterEntities.FindAsync([command.ChapterId], cancellationToken);

        if (chapterEntity == null) throw new ArgumentNullException($"Chapter with ID {command.ChapterId} not found.");

        chapterEntity.Position = command.Position;
        chapterEntity.UpdatedAt = DateTime.UtcNow;
    }
}