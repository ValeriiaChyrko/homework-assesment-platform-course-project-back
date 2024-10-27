using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.DTOs.SharedDTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeAssignment.Persistence.Queries.Users;

public sealed class GetAllUsersByRoleQueryHandler : IRequestHandler<GetAllUsersByRoleQuery, IEnumerable<UserDto>>
{
    private readonly IHomeworkAssignmentDbContext _context;
    private readonly IMapper _mapper;

    public GetAllUsersByRoleQueryHandler(IHomeworkAssignmentDbContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<IEnumerable<UserDto>> Handle(GetAllUsersByRoleQuery query, CancellationToken cancellationToken)
    {
        var users = await _context.UserEntities
            .Where(u => u.RoleType.Equals(query.Role.ToString().ToLower()))
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return users.Select(entityModel => _mapper.Map<UserDto>(entityModel)).ToList();
    }
}