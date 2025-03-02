using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Domain.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeAssignment.Persistence.Queries.Attachments;

public sealed class
    GetAllAttachmentsByCourseIdQueryHandler : IRequestHandler<GetAllAttachmentsByCourseIdQuery,
    IEnumerable<Attachment>>
{
    private readonly IHomeworkAssignmentDbContext _context;
    private readonly IMapper _mapper;

    public GetAllAttachmentsByCourseIdQueryHandler(IHomeworkAssignmentDbContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<IEnumerable<Attachment>> Handle(GetAllAttachmentsByCourseIdQuery query,
        CancellationToken cancellationToken)
    {
        var attempts = await _context
            .AttachmentEntities
            .Where(a => a.CourseId == query.CourseId)
            .OrderBy(a => a.CreatedAt)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return attempts.Select(entityModel => _mapper.Map<Attachment>(entityModel)).ToList();
    }
}