using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Domain.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeAssignment.Persistence.Queries.Attachments;

public sealed class
    GetAllAttachmentsByChapterIdQueryHandler : IRequestHandler<GetAllAttachmentsByChapterIdQuery,
    IEnumerable<Attachment>>
{
    private readonly IHomeworkAssignmentDbContext _context;
    private readonly IMapper _mapper;

    public GetAllAttachmentsByChapterIdQueryHandler(IHomeworkAssignmentDbContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<IEnumerable<Attachment>> Handle(GetAllAttachmentsByChapterIdQuery query,
        CancellationToken cancellationToken)
    {
        var attachmentEntities = await _context
            .AttachmentEntities
            .Where(a => a.ChapterId == query.ChapterId)
            .OrderBy(a => a.CreatedAt)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return attachmentEntities.Select(entityModel => _mapper.Map<Attachment>(entityModel)).ToList();
    }
}