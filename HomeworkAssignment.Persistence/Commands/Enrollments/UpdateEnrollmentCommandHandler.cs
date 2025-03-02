using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Database.Entities;
using HomeAssignment.Domain.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Commands.Enrollments;

public sealed record UpdateEnrollmentCommandHandler : IRequestHandler<UpdateEnrollmentCommand, Enrollment>
{
    private readonly IHomeworkAssignmentDbContext _context;
    private readonly IMapper _mapper;

    public UpdateEnrollmentCommandHandler(IHomeworkAssignmentDbContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public Task<Enrollment> Handle(UpdateEnrollmentCommand command,
        CancellationToken cancellationToken = default)
    {
        if (command is null) throw new ArgumentNullException(nameof(command));
        
        var enrollmentEntity = _mapper.Map<EnrollmentEntity>(command.Enrollment);
        enrollmentEntity.Id = command.Id;
        _context.EnrollmentEntities.Update(enrollmentEntity);

        return Task.FromResult(_mapper.Map<Enrollment>(enrollmentEntity));
    }
}