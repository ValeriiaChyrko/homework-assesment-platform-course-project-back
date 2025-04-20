namespace HomeAssignment.DTOs.RequestDTOs.UserRelated;

public class RequestUserFilterParameters
{
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public string? SortBy { get; set; }
    public bool IsAscending { get; set; } = true;
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}