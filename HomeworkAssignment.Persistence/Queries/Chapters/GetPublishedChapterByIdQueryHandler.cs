using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Domain.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeAssignment.Persistence.Queries.Chapters;

public sealed class GetPublishedChapterByIdQueryHandler : IRequestHandler<GetPublishedChapterByIdQuery, Chapter?>
{
    private readonly IHomeworkAssignmentDbContext _context;
    private readonly IMapper _mapper;

    public GetPublishedChapterByIdQueryHandler(IHomeworkAssignmentDbContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<Chapter?> Handle(GetPublishedChapterByIdQuery query, CancellationToken cancellationToken)
    {
        var chapterEntity = await _context
            .ChapterEntities
            .AsNoTracking()
            .SingleOrDefaultAsync(mr => 
                mr.Id == query.Id 
                && mr.CourseId == query.CourseId
                && mr.IsPublished == true, 
                cancellationToken
            );

        return chapterEntity != null ? _mapper.Map<Chapter>(chapterEntity) : null;
    }
}