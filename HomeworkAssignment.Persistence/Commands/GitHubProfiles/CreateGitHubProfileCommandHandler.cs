using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Database.Entities;
using HomeAssignment.DTOs.SharedDTOs;
using MediatR;

namespace HomeAssignment.Persistence.Commands.GitHubProfiles;

public sealed class CreateGitHubProfileCommandHandler : IRequestHandler<CreateGitHubProfileCommand, GitHubProfileDto>
{
    private readonly IHomeworkAssignmentDbContext _context;
    private readonly IMapper _mapper;

    public CreateGitHubProfileCommandHandler(IHomeworkAssignmentDbContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }


    public async Task<GitHubProfileDto> Handle(CreateGitHubProfileCommand command, CancellationToken cancellationToken)
    {
        if (command is null) throw new ArgumentNullException(nameof(command));

        var gitHubProfileDto = _mapper.Map<GitHubProfilesEntity>(command.GitHubProfileDto);
        await _context.GitHubProfilesEntities.AddAsync(gitHubProfileDto, cancellationToken);

        return _mapper.Map<GitHubProfileDto>(gitHubProfileDto);
    }
}