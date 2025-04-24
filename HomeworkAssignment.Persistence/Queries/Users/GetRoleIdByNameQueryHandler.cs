using HomeAssignment.Database.Contexts.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeAssignment.Persistence.Queries.Users;

public sealed class GetRoleIdByNameQueryHandler(IHomeworkAssignmentDbContext context)
    : IRequestHandler<GetRoleIdByNameQuery, int>
{
    private readonly IHomeworkAssignmentDbContext
        _context = context ?? throw new ArgumentNullException(nameof(context));

    public async Task<int> Handle(GetRoleIdByNameQuery query, CancellationToken cancellationToken)
    {
        var roleId = await _context.RoleEntities
            .AsNoTracking()
            .Where(r => r.Name == query.RoleName)
            .Select(r => (int?)r.Id)
            .SingleOrDefaultAsync(cancellationToken);

        if (roleId is null)
            throw new KeyNotFoundException($"Role with name '{query.RoleName}' not found.");

        return roleId.Value;
    }
}