using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Database.Entities;
using HomeAssignment.Domain.Abstractions;
using HomeAssignment.DTOs.RespondDTOs;
using MediatR;

namespace HomeAssignment.Persistence.Commands.Assignments;

public sealed record UpdateAssignmentCommandHandler : IRequestHandler<UpdateAssignmentCommand, RespondAssignmentDto>
{
    private readonly IHomeworkAssignmentDbContext _context;
    private readonly IMapper _mapper;

    public UpdateAssignmentCommandHandler(IHomeworkAssignmentDbContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public Task<RespondAssignmentDto> Handle(UpdateAssignmentCommand command,
        CancellationToken cancellationToken = default)
    {
        if (command is null)
        {
            throw new ArgumentNullException(nameof(command));
        }

        var compilationSection = _mapper.Map<ScoreSection>(command.AssignmentDto.CompilationSection);
        var testsSection = _mapper.Map<ScoreSection>(command.AssignmentDto.TestsSection);
        var qualitySection = _mapper.Map<ScoreSection>(command.AssignmentDto.QualitySection);
        
        var assignment = Assignment.Create(
            command.AssignmentDto.OwnerId,
            command.AssignmentDto.Title,
            command.AssignmentDto.Description,
            command.AssignmentDto.Deadline,
            command.AssignmentDto.MaxScore,
            command.AssignmentDto.MaxAttemptsAmount,
            compilationSection,
            testsSection,
            qualitySection
        );

        var assignmentEntity = _mapper.Map<AssignmentEntity>(assignment);
        assignmentEntity.Id = command.Id;
        _context.AssignmentEntities.Update(assignmentEntity);

        return Task.FromResult(_mapper.Map<RespondAssignmentDto>(assignmentEntity));
    }
}