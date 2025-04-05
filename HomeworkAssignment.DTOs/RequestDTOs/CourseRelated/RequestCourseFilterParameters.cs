namespace HomeAssignment.DTOs.RequestDTOs.CourseRelated;

public class RequestCourseFilterParameters
{
    public string? Title { get; set; }
    public Guid? CategoryId { get; set; }
    public bool IsPublished { get; set; } = true; 
    public List<string> Include { get; set; } = [];


    public bool IncludeCategory => Include.Contains("category");
    public bool IncludeChapters => Include.Contains("chapters");
    public bool IncludeProgress => Include.Contains("progress");


    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}