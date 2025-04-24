using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Domain.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeAssignment.Persistence.Queries.Chapters;

public sealed class GetAllPublishedChaptersByCourseIdQueryHandler(
    IHomeworkAssignmentDbContext context,
    IMapper mapper)
    : IRequestHandler<GetAllPublishedChaptersByCourseIdQuery, IEnumerable<Chapter>>
{
    public async Task<IEnumerable<Chapter>> Handle(GetAllPublishedChaptersByCourseIdQuery query,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

        var chapterEntities = await context.ChapterEntities
            .AsNoTracking()
            .Where(c => c.CourseId == query.CourseId && c.IsPublished)
            .OrderBy(c => c.Position)
            .ToListAsync(cancellationToken);

        return mapper.Map<IEnumerable<Chapter>>(chapterEntities);
    }
}