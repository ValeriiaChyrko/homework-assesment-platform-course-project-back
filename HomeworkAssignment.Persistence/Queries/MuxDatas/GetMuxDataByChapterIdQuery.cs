using HomeAssignment.Domain.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Queries.MuxDatas;

public record GetMuxDataByChapterIdQuery(Guid ChapterId)
    : IRequest<MuxData?>;