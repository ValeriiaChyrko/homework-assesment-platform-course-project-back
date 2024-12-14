using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.DTOs.RespondDTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeAssignment.Persistence.Queries.Attempts;

public sealed class
    GetAllAttemptsByStudentIdQueryHandler : IRequestHandler<GetAllAttemptsByStudentIdQuery,
    IEnumerable<RespondAttemptDto>>
{
    private readonly IHomeworkAssignmentDbContext _context;
    private readonly IMapper _mapper;

    public GetAllAttemptsByStudentIdQueryHandler(IHomeworkAssignmentDbContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<IEnumerable<RespondAttemptDto>> Handle(GetAllAttemptsByStudentIdQuery query,
        CancellationToken cancellationToken)
    {
        var attempts = await _context
            .AttemptEntities
            .Where(a => a.AssignmentId == query.AssignmentId && a.StudentId == query.StudentId)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return attempts.Select(entityModel => _mapper.Map<RespondAttemptDto>(entityModel)).ToList();
    }
}