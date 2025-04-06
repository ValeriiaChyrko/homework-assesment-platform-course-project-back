using HomeAssignment.Database.Contexts.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeAssignment.Persistence.Queries.UserChapterProgresses;

public sealed class GetUserProgressPercentageQueryHandler 
    : IRequestHandler<GetUserProgressPercentageQuery, int>
{
    private readonly IHomeworkAssignmentDbContext _context;
    private const int PercentageMultiplier = 100;

    public GetUserProgressPercentageQueryHandler(IHomeworkAssignmentDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<int> Handle(GetUserProgressPercentageQuery query, CancellationToken cancellationToken)
    {
        var totalChaptersQuery = _context.ChapterEntities
            .Where(c => c.CourseId == query.CourseId && c.IsPublished);

        var totalChapters = await totalChaptersQuery.CountAsync(cancellationToken);

        if (totalChapters == 0) return 0;

        var completedChapters = await _context.UserChapterProgressEntities
            .Where(up => up.UserId == query.UserId && 
                         totalChaptersQuery.Any(c => c.Id == up.ChapterId) && 
                         up.IsCompleted)
            .CountAsync(cancellationToken);

        return (int)Math.Round((decimal)completedChapters / totalChapters * PercentageMultiplier);
    }
}