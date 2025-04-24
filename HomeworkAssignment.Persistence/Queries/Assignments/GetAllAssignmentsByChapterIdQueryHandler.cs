using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Domain.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeAssignment.Persistence.Queries.Assignments;

public sealed class GetAllAssignmentsByIdQueryHandler(
    IHomeworkAssignmentDbContext context,
    IMapper mapper)
    : IRequestHandler<GetAllAssignmentsByChapterIdQuery, IEnumerable<Assignment>>
{
    public async Task<IEnumerable<Assignment>> Handle(GetAllAssignmentsByChapterIdQuery query,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

        var assignments = await context.AssignmentEntities
            .AsNoTracking()
            .Where(a => a.ChapterId == query.ChapterId)
            .OrderBy(a => a.Position)
            .ToListAsync(cancellationToken);

        return mapper.Map<IEnumerable<Assignment>>(assignments);
    }
}