using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Database.Entities;
using HomeAssignment.Domain.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Commands.Chapters;

public sealed class UpdateChapterCommandHandler(IHomeworkAssignmentDbContext context, IMapper mapper)
    : IRequestHandler<UpdateChapterCommand, Chapter>
{
    private readonly IHomeworkAssignmentDbContext
        _context = context ?? throw new ArgumentNullException(nameof(context));

    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    public Task<Chapter> Handle(UpdateChapterCommand command,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);

        var chapterEntity = _mapper.Map<ChapterEntity>(command.Chapter);
        chapterEntity.Id = command.ChapterId;
        _context.ChapterEntities.Update(chapterEntity);

        return Task.FromResult(_mapper.Map<Chapter>(chapterEntity));
    }
}