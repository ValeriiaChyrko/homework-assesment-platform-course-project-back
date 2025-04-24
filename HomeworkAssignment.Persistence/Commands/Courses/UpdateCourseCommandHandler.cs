using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Database.Entities;
using HomeAssignment.Domain.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Commands.Courses;

public sealed class UpdateCourseCommandHandler(IHomeworkAssignmentDbContext context, IMapper mapper)
    : IRequestHandler<UpdateCourseCommand, Course>
{
    private readonly IHomeworkAssignmentDbContext
        _context = context ?? throw new ArgumentNullException(nameof(context));

    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    public Task<Course> Handle(UpdateCourseCommand command,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);

        var courseEntity = _mapper.Map<CourseEntity>(command.Course);
        courseEntity.Id = command.CourseId;
        _context.CourseEntities.Update(courseEntity);

        return Task.FromResult(_mapper.Map<Course>(courseEntity));
    }
}