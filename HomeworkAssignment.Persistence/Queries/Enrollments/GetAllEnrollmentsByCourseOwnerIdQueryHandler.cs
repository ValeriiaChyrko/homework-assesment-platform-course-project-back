using AutoMapper;
using AutoMapper.QueryableExtensions;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Domain.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeAssignment.Persistence.Queries.Enrollments;

public sealed class GetAllEnrollmentsByCourseOwnerIdQueryHandler(
    IHomeworkAssignmentDbContext context,
    IMapper mapper)
    : IRequestHandler<GetAllEnrollmentsByCourseOwnerIdQuery, IEnumerable<Enrollment>>
{
    public async Task<IEnumerable<Enrollment>> Handle(
        GetAllEnrollmentsByCourseOwnerIdQuery query,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

        return await context.EnrollmentEntities
            .AsNoTracking()
            .Where(e => e.Course != null && e.Course.UserId == query.OwnerId)
            .OrderByDescending(e => e.CreatedAt)
            .Include(e => e.Course)
            .ProjectTo<Enrollment>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}