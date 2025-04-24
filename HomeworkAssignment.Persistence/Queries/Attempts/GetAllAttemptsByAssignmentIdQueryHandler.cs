using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Domain.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeAssignment.Persistence.Queries.Attempts;

public sealed class GetAllAttemptsByAssignmentIdQueryHandler(
    IHomeworkAssignmentDbContext context,
    IMapper mapper)
    : IRequestHandler<GetAllAttemptsByAssignmentIdQuery, IEnumerable<Attempt>>
{
    public async Task<IEnumerable<Attempt>> Handle(GetAllAttemptsByAssignmentIdQuery query,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

        var attempts = await context.AttemptEntities
            .AsNoTracking()
            .Where(a => a.AssignmentId == query.AssignmentId && a.IsCompleted)
            .GroupBy(a => a.UserId)
            .Select(g => g.OrderByDescending(a => a.CreatedAt).First())
            .ToListAsync(cancellationToken);

        return mapper.Map<IEnumerable<Attempt>>(attempts);
    }
}