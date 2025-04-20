using HomeAssignment.Database.Contexts.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeAssignment.Persistence.Queries.Users;

public sealed class GetUserFullnameQueryHandler : IRequestHandler<GetUserFullnameQuery, string?>
{
    private readonly IHomeworkAssignmentDbContext _context;

    public GetUserFullnameQueryHandler(IHomeworkAssignmentDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<string?> Handle(GetUserFullnameQuery query, CancellationToken cancellationToken)
    {
        var user = await _context.UserEntities
            .AsNoTracking()
            .SingleOrDefaultAsync(u => u.Id == query.UserId, cancellationToken);

        if (user == null) return null;

        return user.FullName;
    }
}