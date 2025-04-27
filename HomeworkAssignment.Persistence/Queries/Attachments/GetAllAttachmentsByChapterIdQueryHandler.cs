using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Domain.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeAssignment.Persistence.Queries.Attachments;

public sealed class GetAllAttachmentsByChapterIdQueryHandler(
    IHomeworkAssignmentDbContext context,
    IMapper mapper)
    : IRequestHandler<GetAllAttachmentsByChapterIdQuery, IEnumerable<Attachment>>
{
    public async Task<IEnumerable<Attachment>> Handle(GetAllAttachmentsByChapterIdQuery query,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

        var attachmentDtos = await context.AttachmentEntities
            .Where(a => a.ChapterId == query.ChapterId)
            .OrderBy(a => a.CreatedAt)
            .AsNoTracking()
            .Select(a => mapper.Map<Attachment>(a))
            .ToListAsync(cancellationToken);

        return attachmentDtos;
    }
}