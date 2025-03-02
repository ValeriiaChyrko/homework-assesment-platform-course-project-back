using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Database.Entities;
using HomeAssignment.Domain.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Commands.Chapters;

public sealed class CreateChapterCommandHandler : IRequestHandler<CreateChapterCommand, Chapter>
{
    private readonly IHomeworkAssignmentDbContext _context;
    private readonly IMapper _mapper;

    public CreateChapterCommandHandler(IHomeworkAssignmentDbContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }


    public async Task<Chapter> Handle(CreateChapterCommand command, CancellationToken cancellationToken)
    {
        if (command is null) throw new ArgumentNullException(nameof(command));

        var chapterEntity = _mapper.Map<ChapterEntity>(command.Chapter);
        var addedEntity = await _context.ChapterEntities.AddAsync(chapterEntity, cancellationToken);

        return _mapper.Map<Chapter>(addedEntity.Entity);
    }
}