using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Domain.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeAssignment.Persistence.Queries.Chapters;

public sealed class
    GetAllChaptersByCourseIdQueryHandler : IRequestHandler<GetAllChaptersByCourseIdQuery, IEnumerable<Chapter>>
{
    private readonly IHomeworkAssignmentDbContext _context;
    private readonly IMapper _mapper;

    public GetAllChaptersByCourseIdQueryHandler(IHomeworkAssignmentDbContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<IEnumerable<Chapter>> Handle(GetAllChaptersByCourseIdQuery query,
        CancellationToken cancellationToken)
    {
        var chapterEntities = await _context
            .ChapterEntities
            .Where(c => c.CourseId == query.CourseId)
            .OrderBy(a => a.Position)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return chapterEntities.Select(entityModel => _mapper.Map<Chapter>(entityModel)).ToList();
    }
}