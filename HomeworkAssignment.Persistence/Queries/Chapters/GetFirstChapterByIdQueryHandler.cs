using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Domain.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeAssignment.Persistence.Queries.Chapters;

public sealed class GetFirstChapterByIdQueryHandler : IRequestHandler<GetFirstChapterByIdQuery, Chapter?>
{
    private readonly IHomeworkAssignmentDbContext _context;
    private readonly IMapper _mapper;

    public GetFirstChapterByIdQueryHandler(IHomeworkAssignmentDbContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<Chapter?> Handle(GetFirstChapterByIdQuery query, CancellationToken cancellationToken)
    {
        var chapter = await _context
            .ChapterEntities
            .AsNoTracking()
            .Where(mr => mr.CourseId == query.CourseId)
            .OrderBy(mr => mr.Position)
            .FirstOrDefaultAsync(cancellationToken);

        return chapter != null ? _mapper.Map<Chapter>(chapter) : null;
    }
}