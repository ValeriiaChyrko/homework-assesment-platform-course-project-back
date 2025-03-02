using HomeAssignment.Domain.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Commands.MuxDatas;

public sealed record CreateMuxDataCommand(MuxData MuxData) : IRequest<MuxData>;