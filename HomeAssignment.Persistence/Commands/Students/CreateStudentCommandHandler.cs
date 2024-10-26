using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Database.Entities;
using HomeAssignment.Domain.Abstractions;
using HomeAssignment.DTOs.RespondDTOs;
using MediatR;

namespace HomeAssignment.Persistence.Commands.Students;

public sealed class CreateStudentCommandHandler : IRequestHandler<CreateStudentCommand, RespondStudentDto>
{
    private readonly IHomeworkAssignmentDbContext _context;
    private readonly IMapper _mapper;
    
    public CreateStudentCommandHandler(IHomeworkAssignmentDbContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }
    
    
    public async Task<RespondStudentDto> Handle(CreateStudentCommand command, CancellationToken cancellationToken)
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
        
        var userEntity = _mapper.Map<UserEntity>(student); 
        await _context.UserEntities.AddAsync(userEntity, cancellationToken);
        
        var profileEntity = _mapper.Map<GitHubProfilesEntity>(student);
        await _context.GitHubProfilesEntities.AddAsync(profileEntity, cancellationToken);
        
        return _mapper.Map<RespondStudentDto>(student);
    }
}