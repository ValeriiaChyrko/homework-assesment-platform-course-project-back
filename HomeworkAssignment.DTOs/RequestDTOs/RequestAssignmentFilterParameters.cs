namespace HomeAssignment.DTOs.RequestDTOs;

public class RequestAssignmentFilterParameters
{
    public string? Title { get; set; }
    public Guid? ChapterId { get; set; }
    public bool? IsPublished { get; set; }
    public string? SortBy { get; set; }
    public bool IsAscending { get; set; } = true; 
    public int PageNumber { get; set; } = 1; 
    public int PageSize { get; set; } = 10;
}