using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Domain.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeAssignment.Persistence.Queries.Chapters;

public sealed class GetChapterByIdQueryHandler(
    IHomeworkAssignmentDbContext context,
    IMapper mapper)
    : IRequestHandler<GetChapterByIdQuery, Chapter?>
{
    public async Task<Chapter?> Handle(GetChapterByIdQuery query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

        var chapterEntity = await context.ChapterEntities
            .AsNoTracking()
            .SingleOrDefaultAsync(
                c => c.Id == query.ChapterId && c.CourseId == query.CourseId,
                cancellationToken);

        return chapterEntity is null ? null : mapper.Map<Chapter>(chapterEntity);
    }
}