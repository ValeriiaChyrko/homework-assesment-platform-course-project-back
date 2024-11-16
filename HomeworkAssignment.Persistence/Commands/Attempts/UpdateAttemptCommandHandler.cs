using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Database.Entities;
using HomeAssignment.Domain.Abstractions;
using HomeAssignment.DTOs.RespondDTOs;
using MediatR;

namespace HomeAssignment.Persistence.Commands.Attempts;

public sealed record UpdateAttemptCommandHandler : IRequestHandler<UpdateAttemptCommand, RespondAttemptDto>
{
    private readonly IHomeworkAssignmentDbContext _context;
    private readonly IMapper _mapper;

    public UpdateAttemptCommandHandler(IHomeworkAssignmentDbContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public Task<RespondAttemptDto> Handle(UpdateAttemptCommand command,
        CancellationToken cancellationToken = default)
    {
        if (command is null) throw new ArgumentNullException(nameof(command));

        var attempt = Attempt.Create(
            command.AttemptDto.StudentId,
            command.AttemptDto.AssignmentId,
            command.AttemptDto.BranchName,
            command.AttemptDto.AttemptNumber,
            command.AttemptDto.CompilationScore,
            command.AttemptDto.TestsScore,
            command.AttemptDto.QualityScore
        );

        var attemptEntity = _mapper.Map<AttemptEntity>(attempt);
        attemptEntity.Id = command.Id;
        _context.AttemptEntities.Update(attemptEntity);

        return Task.FromResult(_mapper.Map<RespondAttemptDto>(attemptEntity));
    }
}