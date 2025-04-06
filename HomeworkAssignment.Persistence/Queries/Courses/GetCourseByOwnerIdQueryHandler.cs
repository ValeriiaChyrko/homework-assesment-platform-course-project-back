using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Domain.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeAssignment.Persistence.Queries.Courses;

public sealed class GetCourseByOwnerIdQueryHandler : IRequestHandler<GetCourseByOwnerIdQuery, Course?>
{
    private readonly IHomeworkAssignmentDbContext _context;
    private readonly IMapper _mapper;

    public GetCourseByOwnerIdQueryHandler(IHomeworkAssignmentDbContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<Course?> Handle(GetCourseByOwnerIdQuery query, CancellationToken cancellationToken)
    {
        var courseEntity = await _context
            .CourseEntities
            .AsNoTracking()
            .SingleOrDefaultAsync(mr => 
                mr.Id == query.CourseId && mr.UserId == query.OwnerId, 
                cancellationToken
            );

        return courseEntity != null ? _mapper.Map<Course>(courseEntity) : null;
    }
}