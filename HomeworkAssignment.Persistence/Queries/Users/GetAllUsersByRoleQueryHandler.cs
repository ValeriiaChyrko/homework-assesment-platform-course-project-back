using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.DTOs.SharedDTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeAssignment.Persistence.Queries.Users;

public sealed class GetAllUsersByRoleQueryHandler : IRequestHandler<GetAllUsersByRoleQuery, PagedList<UserDto>>
{
    private readonly IHomeworkAssignmentDbContext _context;
    private readonly IMapper _mapper;

    public GetAllUsersByRoleQueryHandler(IHomeworkAssignmentDbContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<PagedList<UserDto>> Handle(GetAllUsersByRoleQuery query, CancellationToken cancellationToken)
    {
        var usersQuery = _context.UserEntities
            .AsNoTracking()
            .Where(u => u.RoleType.Equals(query.UserRole.ToString().ToLower()));
        
        if (!string.IsNullOrEmpty(query.FilterParameters.FullName))
        {
            usersQuery = usersQuery.Where(u => u.FullName.Contains(query.FilterParameters.FullName));
        }
        
        if (!string.IsNullOrEmpty(query.FilterParameters.Email))
        {
            usersQuery = usersQuery.Where(u => u.Email == query.FilterParameters.Email);
        }
        
        if (!string.IsNullOrEmpty(query.FilterParameters.SortBy))
        {
            usersQuery = query.FilterParameters.IsAscending
                ? usersQuery.OrderBy(a => EF.Property<object>(a, query.FilterParameters.SortBy))
                : usersQuery.OrderByDescending(a => EF.Property<object>(a, query.FilterParameters.SortBy));
        }
        
        var userDtos = usersQuery.Select(entityModel => _mapper.Map<UserDto>(entityModel));
        return await PagedList<UserDto>.CreateAsync(userDtos, query.FilterParameters.PageNumber, query.FilterParameters.PageSize);
    }
}