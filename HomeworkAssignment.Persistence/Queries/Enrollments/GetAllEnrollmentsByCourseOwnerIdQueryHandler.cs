using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Domain.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeAssignment.Persistence.Queries.Enrollments;

public sealed class GetAllEnrollmentsByCourseOwnerIdQueryHandler : IRequestHandler<GetAllEnrollmentsByCourseOwnerIdQuery, IEnumerable<Enrollment>>
{
    private readonly IHomeworkAssignmentDbContext _context;
    private readonly IMapper _mapper;

    public GetAllEnrollmentsByCourseOwnerIdQueryHandler(IHomeworkAssignmentDbContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<IEnumerable<Enrollment>> Handle(GetAllEnrollmentsByCourseOwnerIdQuery query, CancellationToken cancellationToken)
    {
        var enrollmentEntities = await _context
            .EnrollmentEntities
            .Where(a => a.Course != null && a.Course.UserId == query.OwnerId)
            .OrderByDescending(a => a.CreatedAt)
            .AsNoTracking()
            .Include(a => a.Course)
            .ToListAsync(cancellationToken);

        return enrollmentEntities.Select(entityModel => _mapper.Map<Enrollment>(entityModel)).ToList();
    }
}