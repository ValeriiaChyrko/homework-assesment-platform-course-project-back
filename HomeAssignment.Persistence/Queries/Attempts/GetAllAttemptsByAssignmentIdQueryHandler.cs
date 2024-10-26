using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.DTOs.RespondDTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeAssignment.Persistence.Queries.Attempts;

public sealed class GetAllAttemptsByAssignmentIdQueryHandler : IRequestHandler<GetAllAttemptsByAssignmentIdQuery, IEnumerable<RespondAttemptDto>>
{
    private readonly IHomeworkAssignmentDbContext _context;
    private readonly IMapper _mapper;

    public GetAllAttemptsByAssignmentIdQueryHandler(IHomeworkAssignmentDbContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<IEnumerable<RespondAttemptDto>> Handle(GetAllAttemptsByAssignmentIdQuery query,
        CancellationToken cancellationToken)
    {
        var attempts = await _context
            .AttemptEntities
            .Where(a =>a.AssignmentId == query.AssignmentId)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return attempts.Select(entityModel => _mapper.Map<RespondAttemptDto>(entityModel)).ToList();
    }
}