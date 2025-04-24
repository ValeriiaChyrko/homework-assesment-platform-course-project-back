using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Domain.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeAssignment.Persistence.Queries.Courses;

public sealed class GetCourseByOwnerIdQueryHandler(
    IHomeworkAssignmentDbContext context,
    IMapper mapper)
    : IRequestHandler<GetCourseByOwnerIdQuery, Course?>
{
    public async Task<Course?> Handle(GetCourseByOwnerIdQuery query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

        var courseEntity = await context.CourseEntities
            .AsNoTracking()
            .Where(c => c.Id == query.CourseId && c.UserId == query.OwnerId)
            .Select(c => mapper.Map<Course>(c))
            .SingleOrDefaultAsync(cancellationToken);

        return courseEntity;
    }
}