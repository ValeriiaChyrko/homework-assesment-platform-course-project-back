using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Domain.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeAssignment.Persistence.Queries.Attempts;

public sealed class GetAttemptsByIdQueryHandler(
    IHomeworkAssignmentDbContext context,
    IMapper mapper)
    : IRequestHandler<GetAttemptByIdQuery, Attempt?>
{
    public async Task<Attempt?> Handle(GetAttemptByIdQuery query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

        var attempt = await context.AttemptEntities
            .AsNoTracking()
            .SingleOrDefaultAsync(
                a => a.Id == query.AttemptId && a.AssignmentId == query.AssignmentId,
                cancellationToken);

        return attempt is null ? null : mapper.Map<Attempt>(attempt);
    }
}