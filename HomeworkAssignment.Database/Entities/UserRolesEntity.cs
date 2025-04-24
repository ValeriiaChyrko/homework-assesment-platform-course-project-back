namespace HomeAssignment.Database.Entities;

public class UserRolesEntity
{
    public Guid UserId { get; set; }
    public UserEntity User { get; set; }

    public int RoleId { get; set; }
    public RoleEntity Role { get; set; }
}