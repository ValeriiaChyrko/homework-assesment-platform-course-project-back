using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.DTOs.RespondDTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeAssignment.Persistence.Queries.Attempts;

public sealed class GetAttemptByIdQueryHandler : IRequestHandler<GetAttemptByIdQuery, RespondAssignmentDto?>
{
    private readonly IHomeworkAssignmentDbContext _context;
    private readonly IMapper _mapper;

    public GetAttemptByIdQueryHandler(IHomeworkAssignmentDbContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<RespondAssignmentDto?> Handle(GetAttemptByIdQuery query, CancellationToken cancellationToken)
    {
        var attempt = await _context
            .AttemptEntities
            .AsNoTracking()
            .FirstOrDefaultAsync(mr => mr.Id == query.Id, cancellationToken);

        return attempt != null ? _mapper.Map<RespondAssignmentDto>(attempt) : null;
    }
}