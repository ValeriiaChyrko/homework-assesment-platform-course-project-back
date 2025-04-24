using HomeAssignment.Database.Contexts.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeAssignment.Persistence.Queries.Chapters;

public sealed class IsAnyPublishedChapterByCourseIdQueryHandler(
    IHomeworkAssignmentDbContext context)
    : IRequestHandler<IsAnyPublishedChapterByCourseIdQuery, bool>
{
    public async Task<bool> Handle(IsAnyPublishedChapterByCourseIdQuery query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

        return await context.ChapterEntities
            .AsNoTracking()
            .AnyAsync(ch => ch.CourseId == query.CourseId && ch.IsPublished, cancellationToken);
    }
}