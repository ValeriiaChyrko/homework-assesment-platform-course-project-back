using HomeAssignment.Domain.Abstractions;
using HomeAssignment.DTOs.RequestDTOs;
using HomeAssignment.DTOs.RequestDTOs.CourseRelated;
using HomeAssignment.DTOs.SharedDTOs;
using MediatR;

namespace HomeAssignment.Persistence.Queries.Courses;

public class GetAllCoursesQuery : IRequest<PagedList<Course>>
{
    public RequestCourseFilterParameters FilterParameters { get; init; }

    public GetAllCoursesQuery(RequestCourseFilterParameters filterParameters)
    {
        FilterParameters = filterParameters;
    }
}