using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Database.Entities;
using HomeAssignment.Domain.Abstractions;
using HomeAssignment.DTOs.RespondDTOs;
using MediatR;

namespace HomeAssignment.Persistence.Commands.Teachers;

public sealed record UpdateTeacherCommandHandler : IRequestHandler<UpdateTeacherCommand, RespondTeacherDto>
{
    private readonly IHomeworkAssignmentDbContext _context;
    private readonly IMapper _mapper;

    public UpdateTeacherCommandHandler(IHomeworkAssignmentDbContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public Task<RespondTeacherDto> Handle(UpdateTeacherCommand command,
        CancellationToken cancellationToken = default)
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

        teacher.UserId = command.UserId;
        var userEntity = _mapper.Map<UserEntity>(teacher); 
        _context.UserEntities.Update(userEntity);

        teacher.GitHubProfileId = command.GitHubProfileId;
        var profileEntity = _mapper.Map<GitHubProfilesEntity>(teacher);
        _context.GitHubProfilesEntities.Update(profileEntity);

        return Task.FromResult(_mapper.Map<RespondTeacherDto>(teacher));
    }
}