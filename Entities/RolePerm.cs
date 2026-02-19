namespace billing.Entities;

public class RolePerm
{
    public Guid RoleId { get; set; }
    public Role Role { get; set; }

    public Guid PermId { get; set; }
    public Perm Perm { get; set; }
}
