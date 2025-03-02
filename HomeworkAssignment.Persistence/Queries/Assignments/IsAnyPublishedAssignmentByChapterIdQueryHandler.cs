using HomeAssignment.Database.Contexts.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeAssignment.Persistence.Queries.Assignments;

public sealed class IsAnyPublishedAssignmentByChapterIdQueryHandler : IRequestHandler<IsAnyPublishedAssignmentByChapterIdQuery, bool>
{
    private readonly IHomeworkAssignmentDbContext _context;

    public IsAnyPublishedAssignmentByChapterIdQueryHandler(IHomeworkAssignmentDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<bool> Handle(IsAnyPublishedAssignmentByChapterIdQuery query, CancellationToken cancellationToken)
    {
        var assignments = await _context
            .AssignmentEntities
            .AsNoTracking()
            .Where(mr => mr.ChapterId == query.ChapterId && mr.IsPublished)
            .ToListAsync(cancellationToken);

        return assignments.Count != 0;
    }
}