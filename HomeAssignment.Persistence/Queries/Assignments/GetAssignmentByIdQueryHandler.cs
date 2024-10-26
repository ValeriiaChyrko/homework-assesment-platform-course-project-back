using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.DTOs.RespondDTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeAssignment.Persistence.Queries.Assignments;

public sealed class GetAssignmentByIdQueryHandler : IRequestHandler<GetAssignmentByIdQuery, RespondAssignmentDto?>
{
    private readonly IHomeworkAssignmentDbContext _context;
    private readonly IMapper _mapper;

    public GetAssignmentByIdQueryHandler(IHomeworkAssignmentDbContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<RespondAssignmentDto?> Handle(GetAssignmentByIdQuery query, CancellationToken cancellationToken)
    {
        var assignment = await _context
            .AssignmentEntities
            .AsNoTracking()
            .FirstOrDefaultAsync(mr => mr.Id == query.Id, cancellationToken);

        return assignment != null ? _mapper.Map<RespondAssignmentDto>(assignment) : null;
    }
}