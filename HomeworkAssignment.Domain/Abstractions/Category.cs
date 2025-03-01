using System.Data;

namespace HomeAssignment.Domain.Abstractions;

public class Category
{
    private readonly List<Guid> _courseIds;

    public Category(Guid id, string name, List<Guid> courseIds)
    {
        Id = id;
        Name = name;
        _courseIds = courseIds;
    }

    public Guid Id { get; set; }
    public string Name { get; set; }
    public IReadOnlyList<Guid> CourseIds => _courseIds.AsReadOnly();

    public static Category Create(string name, List<Guid>? courseIds = null)
    {
        return new Category(
            Guid.NewGuid(),
            name,
            courseIds ?? []
        );
    }

    public void Update(string name)
    {
        Name = name;
    }
}