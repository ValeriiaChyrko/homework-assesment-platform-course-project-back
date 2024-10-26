using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.DTOs.RespondDTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeAssignment.Persistence.Queries.Assignments;

public sealed class GetAllAssignmentsQueryHandler : IRequestHandler<GetAllAssignmentsQuery, IEnumerable<RespondAssignmentDto>>
{
    private readonly IHomeworkAssignmentDbContext _context;
    private readonly IMapper _mapper;

    public GetAllAssignmentsQueryHandler(IHomeworkAssignmentDbContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<IEnumerable<RespondAssignmentDto>> Handle(GetAllAssignmentsQuery query,
        CancellationToken cancellationToken)
    {
        var assignments = await _context
            .AssignmentEntities
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return assignments.Select(entityModel => _mapper.Map<RespondAssignmentDto>(entityModel)).ToList();
    }
}