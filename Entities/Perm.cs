namespace billing.Entities;

public class Perm
{
    public Guid Id { get; set; }
    public bool IsActive { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Resource { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;

    public ICollection<RolePerm> RolePerm { get; set; } = new List<RolePerm>();
}
