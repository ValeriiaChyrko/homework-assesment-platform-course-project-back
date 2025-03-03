using HomeAssignment.Database.Contexts.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeAssignment.Persistence.Queries.Chapters;

public sealed class IsAnyPublishedChapterByCurseIdQueryHandler : IRequestHandler<IsAnyPublishedChapterByCourseIdQuery, bool>
{
    private readonly IHomeworkAssignmentDbContext _context;

    public IsAnyPublishedChapterByCurseIdQueryHandler(IHomeworkAssignmentDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<bool> Handle(IsAnyPublishedChapterByCourseIdQuery query, CancellationToken cancellationToken)
    {
        var assignments = await _context
            .ChapterEntities
            .AsNoTracking()
            .Where(mr => mr.CourseId == query.CourseId && mr.IsPublished)
            .ToListAsync(cancellationToken);

        return assignments.Count != 0;
    }
}