using HomeAssignment.Database.Contexts.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Commands.Chapters;

public sealed class DeleteChapterCommandHandler : IRequestHandler<DeleteChapterCommand>
{
    private readonly IHomeworkAssignmentDbContext _context;

    public DeleteChapterCommandHandler(IHomeworkAssignmentDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task Handle(DeleteChapterCommand command, CancellationToken cancellationToken)
    {
        if (command is null) throw new ArgumentNullException(nameof(command));

        var chapterEntity = await _context.ChapterEntities.FindAsync([command.Id], cancellationToken);
        if (chapterEntity != null) _context.ChapterEntities.Remove(chapterEntity);
    }
}