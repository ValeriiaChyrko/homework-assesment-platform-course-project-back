using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Domain.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeAssignment.Persistence.Queries.Users;

public sealed class GetUserByIdQueryHandler(IHomeworkAssignmentDbContext context, IMapper mapper)
    : IRequestHandler<GetUserByIdQuery, User?>
{
    private readonly IHomeworkAssignmentDbContext
        _context = context ?? throw new ArgumentNullException(nameof(context));

    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    public async Task<User?> Handle(GetUserByIdQuery query, CancellationToken cancellationToken)
    {
        var userEntity = await _context.UserEntities
            .AsNoTracking()
            .Where(u => u.Id == query.UserId)
            .Select(u => u)
            .SingleOrDefaultAsync(cancellationToken);

        return userEntity == null ? null : _mapper.Map<User>(userEntity);
    }
}