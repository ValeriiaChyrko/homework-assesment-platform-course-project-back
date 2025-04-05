using HomeAssignment.Database.Contexts.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeAssignment.Persistence.Queries.UserChapterProgresses;

public sealed class GetUserProgressCountQueryHandler 
    : IRequestHandler<GetUserProgressCountQuery, int>
{
    private readonly IHomeworkAssignmentDbContext _context;

    public GetUserProgressCountQueryHandler(IHomeworkAssignmentDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<int> Handle(GetUserProgressCountQuery query, CancellationToken cancellationToken)
    {
        return await _context.UserChapterProgressEntities
            .AsNoTracking()
            .Where(up => up.UserId == query.UserId 
                         && query.ChapterIds.Contains((Guid)up.ChapterId!) 
                         && up.IsCompleted == true)
            .CountAsync(cancellationToken);
    }
}