using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.DTOs.SharedDTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeAssignment.Persistence.Queries.GitHubProfiles;

public sealed class GetAllGitHubProfilesByUserIdQueryHandler : IRequestHandler<GetAllGitHubProfilesByUserIdQuery, IEnumerable<GitHubProfileDto>?>
{
    private readonly IHomeworkAssignmentDbContext _context;
    private readonly IMapper _mapper;

    public GetAllGitHubProfilesByUserIdQueryHandler(IHomeworkAssignmentDbContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<IEnumerable<GitHubProfileDto>?> Handle(GetAllGitHubProfilesByUserIdQuery query,
        CancellationToken cancellationToken)
    {
        var profiles = await _context
            .GitHubProfilesEntities
            .Where(g=>g.UserId == query.UserId)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return profiles.Select(entityModel => _mapper.Map<GitHubProfileDto>(entityModel)).ToList();
    }
}