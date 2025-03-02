using MediatR;

namespace HomeAssignment.Persistence.Commands.MuxDatas;

public sealed record DeleteMuxDataCommand(Guid Id) : IRequest;