using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Domain.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeAssignment.Persistence.Queries.Chapters;

public sealed class GetLastChapterByIdQueryHandler(
    IHomeworkAssignmentDbContext context,
    IMapper mapper)
    : IRequestHandler<GetLastChapterByIdQuery, Chapter?>
{
    public async Task<Chapter?> Handle(GetLastChapterByIdQuery query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

        var chapterEntity = await context.ChapterEntities
            .AsNoTracking()
            .Where(c => c.CourseId == query.CourseId)
            .OrderByDescending(c => c.Position)
            .FirstOrDefaultAsync(cancellationToken);

        return chapterEntity is null ? null : mapper.Map<Chapter>(chapterEntity);
    }
}