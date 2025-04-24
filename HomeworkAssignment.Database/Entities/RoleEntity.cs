namespace HomeAssignment.Database.Entities;

public class RoleEntity
{
    public int Id { get; set; }
    public required string Name { get; set; }

    public ICollection<UserRolesEntity>? UserRoles { get; set; }
}