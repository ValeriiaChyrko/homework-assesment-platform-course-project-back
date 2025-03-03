using HomeAssignment.Domain.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Commands.Courses;

public sealed record UpdateCourseCommand(Guid CourseId, Course Course)
    : IRequest<Course>;