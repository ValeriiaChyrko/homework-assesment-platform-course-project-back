using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Domain.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeAssignment.Persistence.Queries.Enrollments;

public sealed class
    GetAllEnrollmentsByUserIdQueryHandler : IRequestHandler<GetAllEnrollmentsByUserIdQuery,
    IEnumerable<Enrollment>>
{
    private readonly IHomeworkAssignmentDbContext _context;
    private readonly IMapper _mapper;

    public GetAllEnrollmentsByUserIdQueryHandler(IHomeworkAssignmentDbContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<IEnumerable<Enrollment>> Handle(GetAllEnrollmentsByUserIdQuery query,
        CancellationToken cancellationToken)
    {
        var attempts = await _context
            .EnrollmentEntities
            .Where(a => a.UserId == query.UserId)
            .OrderByDescending(a => a.CreatedAt)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return attempts.Select(entityModel => _mapper.Map<Enrollment>(entityModel)).ToList();
    }
}