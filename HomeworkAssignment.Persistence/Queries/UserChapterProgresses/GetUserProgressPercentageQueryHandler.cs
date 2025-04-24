using HomeAssignment.Database.Contexts.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeAssignment.Persistence.Queries.UserChapterProgresses;

public sealed class GetUserProgressPercentageQueryHandler(
    IHomeworkAssignmentDbContext context)
    : IRequestHandler<GetUserProgressPercentageQuery, int>
{
    private const int PercentageMultiplier = 100;

    public async Task<int> Handle(GetUserProgressPercentageQuery query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

        var publishedChapterIds = await context.ChapterEntities
            .Where(c => c.CourseId == query.CourseId && c.IsPublished)
            .Select(c => c.Id)
            .ToListAsync(cancellationToken);

        var totalChapters = publishedChapterIds.Count;
        if (totalChapters == 0) return 0;

        var completedChapters = await context.UserChapterProgressEntities
            .CountAsync(
                up => up.ChapterId.HasValue &&
                      publishedChapterIds.Contains(up.ChapterId.Value) &&
                      up.UserId == query.UserId &&
                      up.IsCompleted,
                cancellationToken);

        return (int)Math.Round((decimal)completedChapters / totalChapters * PercentageMultiplier);
    }
}