using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Domain.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeAssignment.Persistence.Queries.Chapters;

public sealed class GetFirstChapterByIdQueryHandler(
    IHomeworkAssignmentDbContext context,
    IMapper mapper)
    : IRequestHandler<GetFirstChapterByIdQuery, Chapter?>
{
    public async Task<Chapter?> Handle(GetFirstChapterByIdQuery query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

        var chapterEntity = await context.ChapterEntities
            .AsNoTracking()
            .Where(c => c.CourseId == query.CourseId)
            .OrderBy(c => c.Position)
            .FirstOrDefaultAsync(cancellationToken);

        return mapper.Map<Chapter>(chapterEntity);
    }
}