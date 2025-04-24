using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Domain.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeAssignment.Persistence.Queries.Attempts;

public sealed class GetAllAttemptsByUserIdQueryHandler(
    IHomeworkAssignmentDbContext context,
    IMapper mapper)
    : IRequestHandler<GetAllAttemptsByUserIdQuery, IEnumerable<Attempt>>
{
    public async Task<IEnumerable<Attempt>> Handle(GetAllAttemptsByUserIdQuery query,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

        var attempts = await context.AttemptEntities
            .AsNoTracking()
            .Where(a => a.AssignmentId == query.AssignmentId && a.UserId == query.UserId)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync(cancellationToken);

        return mapper.Map<IEnumerable<Attempt>>(attempts);
    }
}