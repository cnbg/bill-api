namespace billing.Entities;

public class UserRole
{
    public Guid OrgId { get; set; }
    public Org Org { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; }

    public Guid RoleId { get; set; }
    public Role Role { get; set; }
}
