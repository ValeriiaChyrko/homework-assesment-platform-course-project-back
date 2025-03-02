using HomeAssignment.Domain.Abstractions;
using MediatR;

namespace HomeAssignment.Persistence.Commands.Courses;

public sealed record CreateCourseCommand(Course Course) : IRequest<Course>;