using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Domain.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeAssignment.Persistence.Queries.Assignments;

public sealed class GetAssignmentByIdQueryHandler(
    IHomeworkAssignmentDbContext context,
    IMapper mapper)
    : IRequestHandler<GetAssignmentByIdQuery, Assignment?>
{
    public async Task<Assignment?> Handle(GetAssignmentByIdQuery query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

        var assignment = await context.AssignmentEntities
            .AsNoTracking()
            .SingleOrDefaultAsync(
                a => a.Id == query.AssignmentId && a.ChapterId == query.ChapterId,
                cancellationToken);

        return assignment is null ? null : mapper.Map<Assignment>(assignment);
    }
}