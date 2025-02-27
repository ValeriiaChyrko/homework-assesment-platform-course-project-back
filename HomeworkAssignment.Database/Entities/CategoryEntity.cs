namespace HomeAssignment.Database.Entities;

public class CategoryEntity
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    
    public ICollection<CourseEntity>? Courses { get; set; }
}