using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Database.Entities;
using HomeAssignment.Domain.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Commands.Enrollments;

public sealed class CreateEnrollmentCommandHandler : IRequestHandler<CreateEnrollmentCommand, Enrollment>
{
    private readonly IHomeworkAssignmentDbContext _context;
    private readonly IMapper _mapper;

    public CreateEnrollmentCommandHandler(IHomeworkAssignmentDbContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }


    public async Task<Enrollment> Handle(CreateEnrollmentCommand command, CancellationToken cancellationToken)
    {
        if (command is null) throw new ArgumentNullException(nameof(command));

        var enrollmentEntity = _mapper.Map<EnrollmentEntity>(command.Enrollment);
        var addedEntity = await _context.EnrollmentEntities.AddAsync(enrollmentEntity, cancellationToken);

        return _mapper.Map<Enrollment>(addedEntity.Entity);
    }
}