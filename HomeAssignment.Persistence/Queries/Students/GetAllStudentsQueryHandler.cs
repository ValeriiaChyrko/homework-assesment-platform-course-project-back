using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Domain.Abstractions.Enums;
using HomeAssignment.DTOs.RespondDTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeAssignment.Persistence.Queries.Students
{
    public sealed class GetAllStudentsQueryHandler : IRequestHandler<GetAllStudentsQuery, IEnumerable<RespondStudentDto>>
    {
        private readonly IHomeworkAssignmentDbContext _context;
        private readonly IMapper _mapper;

        public GetAllStudentsQueryHandler(IHomeworkAssignmentDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<RespondStudentDto>> Handle(GetAllStudentsQuery query, CancellationToken cancellationToken)
        {
            var students = await _context.UserEntities
                .Include(u => u.GitHubProfiles)
                .Where(u => u.RoleType.Equals(UserRoles.Student))
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return students.Select(entityModel => 
            {
                var studentDto = _mapper.Map<RespondStudentDto>(entityModel);
                
                var profile = entityModel.GitHubProfiles?.FirstOrDefault();
                if (profile == null)
                {
                    return studentDto;
                }
                
                studentDto.GithubUsername = profile.GithubUsername;
                studentDto.GithubAccessToken = profile.GithubAccessToken;
                studentDto.GithubProfileUrl = profile.GithubProfileUrl;
                studentDto.GithubPictureUrl = profile.GithubPictureUrl;

                return studentDto;
            }).ToList();
        }
    }
}