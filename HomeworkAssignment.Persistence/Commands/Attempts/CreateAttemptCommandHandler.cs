using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Database.Entities;
using HomeAssignment.Domain.Abstractions;
using HomeAssignment.DTOs.RespondDTOs;
using MediatR;

namespace HomeAssignment.Persistence.Commands.Attempts;

public sealed class CreateAttemptCommandHandler : IRequestHandler<CreateAttemptCommand, RespondAttemptDto>
{
    private readonly IHomeworkAssignmentDbContext _context;
    private readonly IMapper _mapper;

    public CreateAttemptCommandHandler(IHomeworkAssignmentDbContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }


    public async Task<RespondAttemptDto> Handle(CreateAttemptCommand command, CancellationToken cancellationToken)
    {
        if (command is null) throw new ArgumentNullException(nameof(command));

        var attempt = Attempt.Create(
            command.AttemptDto.StudentId,
            command.AttemptDto.AssignmentId,
            command.AttemptDto.AttemptNumber,
            command.AttemptDto.CompilationScore,
            command.AttemptDto.TestsScore,
            command.AttemptDto.QualityScore
        );
        
        var attemptEntity = _mapper.Map<AttemptEntity>(attempt);
        var addedEntity = await _context.AttemptEntities.AddAsync(attemptEntity, cancellationToken);

        return _mapper.Map<RespondAttemptDto>(addedEntity.Entity);
    }
}