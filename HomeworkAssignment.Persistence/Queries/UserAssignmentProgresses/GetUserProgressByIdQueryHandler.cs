using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Domain.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeAssignment.Persistence.Queries.UserAssignmentProgresses;

public sealed class GetUserProgressByIdQueryHandler(
    IHomeworkAssignmentDbContext context,
    IMapper mapper)
    : IRequestHandler<GetAssignmentUserProgressByIdQuery, AssignmentUserProgress?>
{
    public async Task<AssignmentUserProgress?> Handle(
        GetAssignmentUserProgressByIdQuery query,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

        var progressEntity = await context.UserAssignmentProgressEntities
            .AsNoTracking()
            .SingleOrDefaultAsync(
                a => a.AssignmentId == query.AssignmentId && a.UserId == query.UserId,
                cancellationToken);

        return progressEntity is not null
            ? mapper.Map<AssignmentUserProgress>(progressEntity)
            : null;
    }
}