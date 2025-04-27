using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Domain.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeAssignment.Persistence.Queries.Assignments;

public sealed class GetLastAssignmentByIdQueryHandler(
    IHomeworkAssignmentDbContext context,
    IMapper mapper)
    : IRequestHandler<GetLastAssignmentByIdQuery, Assignment?>
{
    public async Task<Assignment?> Handle(GetLastAssignmentByIdQuery query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

        var assignment = await context.AssignmentEntities
            .AsNoTracking()
            .Where(a => a.ChapterId == query.ChapterId)
            .OrderByDescending(a => a.Position)
            .FirstOrDefaultAsync(cancellationToken);

        return assignment is null ? null : mapper.Map<Assignment>(assignment);
    }
}