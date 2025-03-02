using HomeAssignment.Domain.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Commands.MuxDatas;

public sealed record UpdateMuxDataCommand(Guid Id, MuxData MuxData) : IRequest<MuxData>;