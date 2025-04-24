using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Domain.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeAssignment.Persistence.Queries.UserChapterProgresses;

public sealed class GetUserProgressByIdQueryHandler(
    IHomeworkAssignmentDbContext context,
    IMapper mapper)
    : IRequestHandler<GetUserChapterProgressByIdQuery, ChapterUserProgress?>
{
    public async Task<ChapterUserProgress?> Handle(
        GetUserChapterProgressByIdQuery query,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

        var progressEntity = await context.UserChapterProgressEntities
            .AsNoTracking()
            .SingleOrDefaultAsync(
                p => p.ChapterId == query.ChapterId && p.UserId == query.UserId,
                cancellationToken);

        return progressEntity is not null
            ? mapper.Map<ChapterUserProgress>(progressEntity)
            : null;
    }
}