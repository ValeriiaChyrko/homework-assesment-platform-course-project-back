using HomeAssignment.Database.Contexts.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeAssignment.Persistence.Queries.Users;

public sealed class GetUserFullnameQueryHandler(IHomeworkAssignmentDbContext context)
    : IRequestHandler<GetUserFullnameQuery, string?>
{
    private readonly IHomeworkAssignmentDbContext
        _context = context ?? throw new ArgumentNullException(nameof(context));

    public async Task<string?> Handle(GetUserFullnameQuery query, CancellationToken cancellationToken)
    {
        return await _context.UserEntities
            .AsNoTracking()
            .Where(u => u.Id == query.UserId)
            .Select(u => u.FullName)
            .SingleOrDefaultAsync(cancellationToken);
    }
}