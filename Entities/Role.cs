namespace billing.Entities;

public class Role
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }

    public Guid? OrgId { get; set; }
    public Org? Org { get; set; }

    public ICollection<UserRole> UserRole { get; set; } = new List<UserRole>();
    public ICollection<RolePerm> RolePerm { get; set; } = new List<RolePerm>();
}
