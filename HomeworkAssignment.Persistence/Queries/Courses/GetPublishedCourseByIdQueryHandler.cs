using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Domain.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeAssignment.Persistence.Queries.Courses;

public sealed class GetPublishedCourseByIdQueryHandler : IRequestHandler<GetPublishedCourseByIdQuery, Course?>
{
    private readonly IHomeworkAssignmentDbContext _context;
    private readonly IMapper _mapper;

    public GetPublishedCourseByIdQueryHandler(IHomeworkAssignmentDbContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<Course?> Handle(GetPublishedCourseByIdQuery query, CancellationToken cancellationToken)
    {
        var assignment = await _context
            .CourseEntities
            .AsNoTracking()
            .SingleOrDefaultAsync(mr => 
                mr.Id == query.Id 
                && mr.UserId == query.OwnerId
                && mr.IsPublished == true, 
                cancellationToken
            );

        return assignment != null ? _mapper.Map<Course>(assignment) : null;
    }
}