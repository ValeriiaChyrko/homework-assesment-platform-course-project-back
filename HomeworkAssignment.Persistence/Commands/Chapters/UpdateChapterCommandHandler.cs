using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Database.Entities;
using HomeAssignment.Domain.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Commands.Chapters;

public sealed record UpdateChapterCommandHandler : IRequestHandler<UpdateChapterCommand, Chapter>
{
    private readonly IHomeworkAssignmentDbContext _context;
    private readonly IMapper _mapper;

    public UpdateChapterCommandHandler(IHomeworkAssignmentDbContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public Task<Chapter> Handle(UpdateChapterCommand command,
        CancellationToken cancellationToken = default)
    {
        if (command is null) throw new ArgumentNullException(nameof(command));

        var chapterEntity = _mapper.Map<ChapterEntity>(command.Chapter);
        chapterEntity.Id = command.Id;
        _context.ChapterEntities.Update(chapterEntity);

        return Task.FromResult(_mapper.Map<Chapter>(chapterEntity));
    }
}