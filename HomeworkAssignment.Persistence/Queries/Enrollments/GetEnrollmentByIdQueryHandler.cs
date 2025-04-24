using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Domain.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeAssignment.Persistence.Queries.Enrollments;

public sealed class GetEnrollmentByIdQueryHandler(
    IHomeworkAssignmentDbContext context,
    IMapper mapper)
    : IRequestHandler<GetEnrollmentByIdQuery, Enrollment?>
{
    public async Task<Enrollment?> Handle(
        GetEnrollmentByIdQuery query,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

        var enrollmentEntity = await context
            .EnrollmentEntities
            .AsNoTracking()
            .Where(mr => mr.CourseId == query.CourseId && mr.UserId == query.UserId)
            .SingleOrDefaultAsync(cancellationToken);

        return enrollmentEntity != null ? mapper.Map<Enrollment>(enrollmentEntity) : null;
    }
}