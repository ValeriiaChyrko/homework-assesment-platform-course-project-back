using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Database.Entities;
using HomeAssignment.Domain.Abstractions;
using HomeAssignment.DTOs.RespondDTOs;
using MediatR;

namespace HomeAssignment.Persistence.Commands.Students;

public sealed record UpdateStudentCommandHandler : IRequestHandler<UpdateStudentCommand, RespondStudentDto>
{
    private readonly IHomeworkAssignmentDbContext _context;
    private readonly IMapper _mapper;

    public UpdateStudentCommandHandler(IHomeworkAssignmentDbContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public Task<RespondStudentDto> Handle(UpdateStudentCommand command,
        CancellationToken cancellationToken = default)
    {
        if (command is null)
        {
            throw new ArgumentNullException(nameof(command));
        }
        
        var student = Student.Create(
            command.StudentDto.FullName,
            command.StudentDto.Email,
            command.StudentDto.Password,
            command.StudentDto.GithubUsername,
            command.StudentDto.GithubAccessToken,
            command.StudentDto.GithubProfileUrl,
            command.StudentDto.GithubPictureUrl
        );

        student.UserId = command.UserId;
        var userEntity = _mapper.Map<UserEntity>(student); 
        _context.UserEntities.Update(userEntity);

        student.GitHubProfileId = command.GitHubProfileId;
        var profileEntity = _mapper.Map<GitHubProfilesEntity>(student);
        _context.GitHubProfilesEntities.Update(profileEntity);

        return Task.FromResult(_mapper.Map<RespondStudentDto>(student));
    }
}