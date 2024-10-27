using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Database.Entities;
using HomeAssignment.DTOs.SharedDTOs;
using MediatR;

namespace HomeAssignment.Persistence.Commands.GitHubProfiles;

public sealed record UpdateGitHubProfileCommandHandler : IRequestHandler<UpdateGitHubProfileCommand, GitHubProfileDto>
{
    private readonly IHomeworkAssignmentDbContext _context;
    private readonly IMapper _mapper;

    public UpdateGitHubProfileCommandHandler(IHomeworkAssignmentDbContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public Task<GitHubProfileDto> Handle(UpdateGitHubProfileCommand command,
        CancellationToken cancellationToken = default)
    {
        if (command is null) throw new ArgumentNullException(nameof(command));

        var gitHubProfilesEntity = _mapper.Map<GitHubProfilesEntity>(command.GitHubProfileDto);
        _context.GitHubProfilesEntities.Update(gitHubProfilesEntity);

        return Task.FromResult(_mapper.Map<GitHubProfileDto>(gitHubProfilesEntity));
    }
}