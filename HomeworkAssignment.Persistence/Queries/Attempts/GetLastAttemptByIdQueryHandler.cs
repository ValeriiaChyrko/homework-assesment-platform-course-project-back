using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Domain.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeAssignment.Persistence.Queries.Attempts;

public sealed class GetLastAttemptByIdQueryHandler(
    IHomeworkAssignmentDbContext context,
    IMapper mapper)
    : IRequestHandler<GetLastAttemptByIdQuery, Attempt?>
{
    public async Task<Attempt?> Handle(GetLastAttemptByIdQuery query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

        var lastAttempt = await context.AttemptEntities
            .AsNoTracking()
            .Where(a => a.UserId == query.UserId && a.AssignmentId == query.AssignmentId)
            .OrderByDescending(a => a.Position)
            .FirstOrDefaultAsync(cancellationToken);

        return lastAttempt is null ? null : mapper.Map<Attempt>(lastAttempt);
    }
}