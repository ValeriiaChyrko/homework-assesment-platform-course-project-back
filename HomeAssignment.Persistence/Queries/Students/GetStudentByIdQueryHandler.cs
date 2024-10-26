using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Domain.Abstractions.Enums;
using HomeAssignment.DTOs.RespondDTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeAssignment.Persistence.Queries.Students;

public sealed class GetStudentByIdQueryHandler : IRequestHandler<GetStudentByIdQuery, RespondStudentDto?>
{
    private readonly IHomeworkAssignmentDbContext _context;
    private readonly IMapper _mapper;

    public GetStudentByIdQueryHandler(IHomeworkAssignmentDbContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<RespondStudentDto?> Handle(GetStudentByIdQuery query, CancellationToken cancellationToken)
    {
        var student = await _context.UserEntities
            .Include(u => u.GitHubProfiles) 
            .Where(u => u.Id == query.Id && u.RoleType.Equals(UserRoles.Student))
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);

        if (student == null)
        {
            return null; 
        }
        
        var studentDto = _mapper.Map<RespondStudentDto>(student);
        
        var profile = student.GitHubProfiles?.FirstOrDefault();
        if (profile == null)
        {
            return studentDto;
        }
        
        studentDto.GithubUsername = profile.GithubUsername;
        studentDto.GithubAccessToken = profile.GithubAccessToken;
        studentDto.GithubProfileUrl = profile.GithubProfileUrl;
        studentDto.GithubPictureUrl = profile.GithubPictureUrl;

        return studentDto;
    }
}