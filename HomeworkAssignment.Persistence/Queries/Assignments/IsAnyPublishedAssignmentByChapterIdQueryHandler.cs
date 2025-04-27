using HomeAssignment.Database.Contexts.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeAssignment.Persistence.Queries.Assignments;

public sealed class IsAnyPublishedAssignmentByChapterIdQueryHandler(
    IHomeworkAssignmentDbContext context)
    : IRequestHandler<IsAnyPublishedAssignmentByChapterIdQuery, bool>
{
    public async Task<bool> Handle(IsAnyPublishedAssignmentByChapterIdQuery query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

        return await context.AssignmentEntities
            .AsNoTracking()
            .AnyAsync(a => a.ChapterId == query.ChapterId && a.IsPublished, cancellationToken);
    }
}