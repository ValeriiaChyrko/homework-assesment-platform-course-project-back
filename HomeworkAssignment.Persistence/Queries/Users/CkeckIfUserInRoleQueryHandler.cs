using HomeAssignment.Database.Contexts.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeAssignment.Persistence.Queries.Users;

public sealed class CheckIfUserInRoleQueryHandler : IRequestHandler<CheckIfUserInRoleQuery, bool>
{
    private readonly IHomeworkAssignmentDbContext _context;

    public CheckIfUserInRoleQueryHandler(IHomeworkAssignmentDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<bool> Handle(CheckIfUserInRoleQuery query, CancellationToken cancellationToken)
    {
        var user = await _context.UserEntities
            .AsNoTracking()
            .FirstOrDefaultAsync(
                u => u.RoleType.Equals(query.UserRole.ToString(), StringComparison.OrdinalIgnoreCase) &&
                     u.Id == query.UserId, cancellationToken);

        return user != null;
    }
}