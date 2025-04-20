using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Domain.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeAssignment.Persistence.Queries.Chapters;

public sealed class
    GetAllPublishedChaptersByCourseIdQueryHandler : IRequestHandler<GetAllPublishedChaptersByCourseIdQuery,
    IEnumerable<Chapter>>
{
    private readonly IHomeworkAssignmentDbContext _context;
    private readonly IMapper _mapper;

    public GetAllPublishedChaptersByCourseIdQueryHandler(IHomeworkAssignmentDbContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<IEnumerable<Chapter>> Handle(GetAllPublishedChaptersByCourseIdQuery query,
        CancellationToken cancellationToken)
    {
        var chapterEntities = await _context
            .ChapterEntities
            .Where(c => c.CourseId == query.CourseId && c.IsPublished)
            .OrderBy(a => a.Position)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return chapterEntities.Select(entityModel => _mapper.Map<Chapter>(entityModel)).ToList();
    }
}