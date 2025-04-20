using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Domain.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeAssignment.Persistence.Queries.Chapters;

public sealed class GetChapterByIdQueryHandler : IRequestHandler<GetChapterByIdQuery, Chapter?>
{
    private readonly IHomeworkAssignmentDbContext _context;
    private readonly IMapper _mapper;

    public GetChapterByIdQueryHandler(IHomeworkAssignmentDbContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<Chapter?> Handle(GetChapterByIdQuery query, CancellationToken cancellationToken)
    {
        var chapter = await _context
            .ChapterEntities
            .AsNoTracking()
            .SingleOrDefaultAsync(mr =>
                    mr.Id == query.ChapterId && mr.CourseId == query.CourseId,
                cancellationToken
            );

        return chapter != null ? _mapper.Map<Chapter>(chapter) : null;
    }
}