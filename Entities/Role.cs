namespace billing.Entities;

public class Role
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }
    public List<string> Perms { get; set; } = [];

    public Guid OrgId { get; set; }
    public Org Org { get; set; }
}
