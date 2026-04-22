namespace billing.Entities;

public class Org
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Decimal Balance { get; set; } = 0;
    public string? Inn { get; set; }
    public string? Okpo { get; set; }
    public bool IsActive { get; set; }
    public string? Note { get; set; }

    public Guid OrgTypeId { get; set; }
    public OrgType OrgType { get; set; }
}
