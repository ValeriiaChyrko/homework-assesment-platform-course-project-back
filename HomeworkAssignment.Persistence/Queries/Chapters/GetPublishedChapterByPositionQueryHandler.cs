using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Domain.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeAssignment.Persistence.Queries.Chapters;

public sealed class GetPublishedChapterByPositionQueryHandler(
    IHomeworkAssignmentDbContext context,
    IMapper mapper)
    : IRequestHandler<GetPublishedChapterByPositionQuery, Chapter?>
{
    public async Task<Chapter?> Handle(GetPublishedChapterByPositionQuery query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

        var chapterEntity = await context.ChapterEntities
            .AsNoTracking()
            .SingleOrDefaultAsync(
                c => c.Position == query.Position &&
                     c.CourseId == query.CourseId &&
                     c.IsPublished,
                cancellationToken
            );

        return chapterEntity is null ? null : mapper.Map<Chapter>(chapterEntity);
    }
}