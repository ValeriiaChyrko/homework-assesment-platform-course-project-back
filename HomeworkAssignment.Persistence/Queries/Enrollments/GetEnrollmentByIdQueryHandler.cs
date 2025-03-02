using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Domain.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeAssignment.Persistence.Queries.Enrollments;

public sealed class GetEnrollmentByIdQueryHandler : IRequestHandler<GetEnrollmentByIdQuery, Enrollment?>
{
    private readonly IHomeworkAssignmentDbContext _context;
    private readonly IMapper _mapper;

    public GetEnrollmentByIdQueryHandler(IHomeworkAssignmentDbContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<Enrollment?> Handle(GetEnrollmentByIdQuery query, CancellationToken cancellationToken)
    {
        var enrollment = await _context
            .EnrollmentEntities
            .AsNoTracking()
            .SingleOrDefaultAsync(mr => 
                mr.UserId == query.UserId && mr.CourseId == query.CourseId, 
                cancellationToken
            );

        return enrollment != null ? _mapper.Map<Enrollment>(enrollment) : null;
    }
}