namespace HomeAssignment.Domain.Abstractions
{
    public class Category(Guid id, string name, List<Guid>? courseIds = null)
    {
        private readonly List<Guid> _courseIds = courseIds ?? [];

        public Guid Id { get; init; } = id;
        public string Name { get; private set; } = name;

        public IReadOnlyList<Guid> CourseIds => _courseIds.AsReadOnly();

        public static Category Create(string name, List<Guid>? courseIds = null)
        {
            return new Category(
                Guid.NewGuid(),
                name,
                courseIds
            );
        }
        
        public void Update(string name)
        {
            Name = name;
        }
    }
}