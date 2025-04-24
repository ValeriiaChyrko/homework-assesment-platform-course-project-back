using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Domain.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeAssignment.Persistence.Queries.Courses;

public sealed class GetCoursesByIdsQueryHandler(
    IHomeworkAssignmentDbContext context,
    IMapper mapper)
    : IRequestHandler<GetCoursesByIdsQuery, List<Course>>
{
    public async Task<List<Course>> Handle(GetCoursesByIdsQuery query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

        if (query.CourseIds.Count == 0) return [];

        var courseEntities = await context.CourseEntities
            .AsNoTracking()
            .Where(x => query.CourseIds.Contains(x.Id) && x.IsPublished)
            .ToListAsync(cancellationToken);

        return mapper.Map<List<Course>>(courseEntities);
    }
}