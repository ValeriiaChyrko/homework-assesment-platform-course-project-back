using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.DTOs.SharedDTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeAssignment.Persistence.Queries.GitHubProfiles;

public sealed class GetGitHubProfileByIdQueryHandler : IRequestHandler<GetGitHubProfileByIdQuery, GitHubProfileDto?>
{
    private readonly IHomeworkAssignmentDbContext _context;
    private readonly IMapper _mapper;

    public GetGitHubProfileByIdQueryHandler(IHomeworkAssignmentDbContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<GitHubProfileDto?> Handle(GetGitHubProfileByIdQuery query, CancellationToken cancellationToken)
    {
        var user = await _context
            .GitHubProfilesEntities
            .AsNoTracking()
            .FirstOrDefaultAsync(mr => mr.Id == query.Id, cancellationToken);

        return user != null ? _mapper.Map<GitHubProfileDto>(user) : null;
    }
}