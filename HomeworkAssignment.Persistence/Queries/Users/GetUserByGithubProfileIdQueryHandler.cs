using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.DTOs.SharedDTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeAssignment.Persistence.Queries.Users;

public sealed class GetUserByGithubProfileIdQueryHandler : IRequestHandler<GetUserByGithubProfileIdQuery, UserDto?>
{
    private readonly IHomeworkAssignmentDbContext _context;
    private readonly IMapper _mapper;

    public GetUserByGithubProfileIdQueryHandler(IHomeworkAssignmentDbContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<UserDto?> Handle(GetUserByGithubProfileIdQuery query, CancellationToken cancellationToken)
    {
        var gitHubProfileEntity = await _context
            .GitHubProfilesEntities
            .AsNoTracking()
            .FirstOrDefaultAsync(mr => mr.Id == query.GithubProfileId, cancellationToken);
        if (gitHubProfileEntity is null) return null;

        var user = await _context
            .UserEntities
            .AsNoTracking()
            .FirstOrDefaultAsync(mr => mr.Id == gitHubProfileEntity.UserId, cancellationToken);

        return user is not null ? _mapper.Map<UserDto>(user) : null;
    }
}