using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Domain.Abstractions.Enums;
using HomeAssignment.DTOs.RespondDTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeAssignment.Persistence.Queries.Teachers;

public sealed class GetTeacherByIdQueryHandler : IRequestHandler<GetTeacherByIdQuery, RespondTeacherDto?>
{
    private readonly IHomeworkAssignmentDbContext _context;
    private readonly IMapper _mapper;

    public GetTeacherByIdQueryHandler(IHomeworkAssignmentDbContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<RespondTeacherDto?> Handle(GetTeacherByIdQuery query, CancellationToken cancellationToken)
    {
        var teacher = await _context.UserEntities
            .Include(u => u.GitHubProfiles) 
            .Where(u => u.Id == query.Id && u.RoleType.Equals(UserRoles.Student))
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);

        if (teacher == null)
        {
            return null; 
        }
        
        var teacherDto = _mapper.Map<RespondTeacherDto>(teacher);
        
        var profile = teacher.GitHubProfiles?.FirstOrDefault();
        if (profile == null)
        {
            return teacherDto;
        }
        
        teacherDto.GithubUsername = profile.GithubUsername;
        teacherDto.GithubAccessToken = profile.GithubAccessToken;
        teacherDto.GithubProfileUrl = profile.GithubProfileUrl;
        teacherDto.GithubPictureUrl = profile.GithubPictureUrl;

        return teacherDto;
    }
}