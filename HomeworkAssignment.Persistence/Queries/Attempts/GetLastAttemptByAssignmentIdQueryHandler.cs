using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.DTOs.RespondDTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeAssignment.Persistence.Queries.Attempts;

public sealed class
    GetLastAttemptByAssignmentIdQueryHandler : IRequestHandler<GetLastAttemptByAssignmentIdQuery, RespondAttemptDto>
{
    private readonly IHomeworkAssignmentDbContext _context;
    private readonly IMapper _mapper;

    public GetLastAttemptByAssignmentIdQueryHandler(IHomeworkAssignmentDbContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper;
    }

    public async Task<RespondAttemptDto> Handle(GetLastAttemptByAssignmentIdQuery query,
        CancellationToken cancellationToken)
    {
        var lastAttempt = await _context
            .AttemptEntities
            .Where(a => a.AssignmentId == query.AssignmentId)
            .AsNoTracking()
            .OrderBy(a => a.FinishedAt)
            .LastOrDefaultAsync(cancellationToken);

        return _mapper.Map<RespondAttemptDto>(lastAttempt);
    }
}