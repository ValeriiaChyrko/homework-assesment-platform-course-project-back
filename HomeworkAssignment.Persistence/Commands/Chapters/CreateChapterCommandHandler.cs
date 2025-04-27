using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Database.Entities;
using HomeAssignment.Domain.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Commands.Chapters;

public sealed class CreateChapterCommandHandler(IHomeworkAssignmentDbContext context, IMapper mapper)
    : IRequestHandler<CreateChapterCommand, Chapter>
{
    private readonly IHomeworkAssignmentDbContext
        _context = context ?? throw new ArgumentNullException(nameof(context));

    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));


    public async Task<Chapter> Handle(CreateChapterCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        var chapterEntity = _mapper.Map<ChapterEntity>(command.Chapter);
        var addedEntity = await _context.ChapterEntities.AddAsync(chapterEntity, cancellationToken);

        return _mapper.Map<Chapter>(addedEntity.Entity);
    }
}