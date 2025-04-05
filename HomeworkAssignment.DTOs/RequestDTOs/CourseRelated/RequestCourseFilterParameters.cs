namespace HomeAssignment.DTOs.RequestDTOs.CourseRelated;

public class RequestCourseFilterParameters
{
    public string? Title { get; set; }
    public Guid? CategoryId { get; set; }
    public bool IsPublished { get; set; } = true; 
    public string? Include { get; set; }


    public bool IncludeCategory => !string.IsNullOrEmpty(Include) && Include.Contains("category");
    public bool IncludeChapters => !string.IsNullOrEmpty(Include) && Include.Contains("chapters");
    public bool IncludeProgress => !string.IsNullOrEmpty(Include) && Include.Contains("progress");


    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
