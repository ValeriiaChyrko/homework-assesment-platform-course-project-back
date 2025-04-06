namespace HomeAssignment.DTOs.RequestDTOs.CourseRelated;

public class RequestCourseFilterParameters
{
    public string? Title { get; set; }
    public Guid? CategoryId { get; set; }
    public bool IsPublished { get; set; } = true; 
    public List<string> Include { get; set; } = [];
    public List<string> FilterBy { get; set; } = [];


    public bool IncludeCategory => Include.Contains("category");
    public bool IncludeChapters => Include.Contains("chapters");
    public bool IncludeAttachments => Include.Contains("attachments");
    public bool IncludeProgress => Include.Contains("progress");
    public bool FilterByStudent => FilterBy.Contains("student");

    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}