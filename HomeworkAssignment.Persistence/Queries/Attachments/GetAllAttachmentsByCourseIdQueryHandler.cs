using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Domain.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeAssignment.Persistence.Queries.Attachments;

public sealed class GetAllAttachmentsByCourseIdQueryHandler(
    IHomeworkAssignmentDbContext context,
    IMapper mapper)
    : IRequestHandler<GetAllAttachmentsByCourseIdQuery, IEnumerable<Attachment>>
{
    public async Task<IEnumerable<Attachment>> Handle(GetAllAttachmentsByCourseIdQuery query,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

        var attachmentDtos = await context.AttachmentEntities
            .Where(a => a.CourseId == query.CourseId)
            .OrderBy(a => a.CreatedAt)
            .AsNoTracking()
            .Select(a => mapper.Map<Attachment>(a))
            .ToListAsync(cancellationToken);

        return attachmentDtos;
    }
}