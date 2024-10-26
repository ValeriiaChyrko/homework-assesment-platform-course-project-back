using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Domain.Abstractions.Enums;
using HomeAssignment.DTOs.RespondDTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeAssignment.Persistence.Queries.Teachers
{
    public sealed class GetAllTeachersQueryHandler : IRequestHandler<GetAllTeachersQuery, IEnumerable<RespondTeacherDto>>
    {
        private readonly IHomeworkAssignmentDbContext _context;
        private readonly IMapper _mapper;

        public GetAllTeachersQueryHandler(IHomeworkAssignmentDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<RespondTeacherDto>> Handle(GetAllTeachersQuery query, CancellationToken cancellationToken)
        {
            var teachers = await _context.UserEntities
                .Include(u => u.GitHubProfiles)
                .Where(u => u.RoleType.Equals(UserRoles.Teacher))
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return teachers.Select(entityModel => 
            {
                var teacherDto = _mapper.Map<RespondTeacherDto>(entityModel);
                
                var profile = entityModel.GitHubProfiles?.FirstOrDefault();
                if (profile == null)
                {
                    return teacherDto;
                }
                
                teacherDto.GithubUsername = profile.GithubUsername;
                teacherDto.GithubAccessToken = profile.GithubAccessToken;
                teacherDto.GithubProfileUrl = profile.GithubProfileUrl;
                teacherDto.GithubPictureUrl = profile.GithubPictureUrl;

                return teacherDto;
            }).ToList();
        }
    }
}