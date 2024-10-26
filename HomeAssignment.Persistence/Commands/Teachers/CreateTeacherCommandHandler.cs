using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Database.Entities;
using HomeAssignment.Domain.Abstractions;
using HomeAssignment.DTOs.RespondDTOs;
using MediatR;

namespace HomeAssignment.Persistence.Commands.Teachers;

public sealed class CreateTeacherCommandHandler : IRequestHandler<CreateTeacherCommand, RespondTeacherDto>
{
    private readonly IHomeworkAssignmentDbContext _context;
    private readonly IMapper _mapper;
    
    public CreateTeacherCommandHandler(IHomeworkAssignmentDbContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }
    
    
    public async Task<RespondTeacherDto> Handle(CreateTeacherCommand command, CancellationToken cancellationToken)
    {
        if (command is null)
        {
            throw new ArgumentNullException(nameof(command));
        }
        
        var teacher = Teacher.Create(
            command.TeacherDto.FullName,
            command.TeacherDto.Email,
            command.TeacherDto.Password,
            command.TeacherDto.GithubUsername,
            command.TeacherDto.GithubAccessToken,
            command.TeacherDto.GithubProfileUrl,
            command.TeacherDto.GithubPictureUrl
        );
        
        var userEntity = _mapper.Map<UserEntity>(teacher); 
        await _context.UserEntities.AddAsync(userEntity, cancellationToken);
        
        var profileEntity = _mapper.Map<GitHubProfilesEntity>(teacher);
        await _context.GitHubProfilesEntities.AddAsync(profileEntity, cancellationToken);
        
        return _mapper.Map<RespondTeacherDto>(teacher);
    }
}