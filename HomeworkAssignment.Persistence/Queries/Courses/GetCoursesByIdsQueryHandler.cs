using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Domain.Abstractions;
using HomeAssignment.Persistence.Queries.Enrollments;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeAssignment.Persistence.Queries.Courses;

public sealed class GetCoursesByIdsQueryHandler : IRequestHandler<GetCoursesByIdsQuery, List<Course>>
{
    private readonly IHomeworkAssignmentDbContext _context;
    private readonly IMapper _mapper;

    public GetCoursesByIdsQueryHandler(IHomeworkAssignmentDbContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<List<Course>> Handle(GetCoursesByIdsQuery query, CancellationToken cancellationToken)
    {
        var courseEntities = await _context
            .CourseEntities
            .AsNoTracking()
            .Where(x => query.CourseIds.Contains(x.Id) && x.IsPublished)
            .ToListAsync(cancellationToken);

        return courseEntities.Count == 0 ? [] : _mapper.Map<List<Course>>(courseEntities);
    }
}