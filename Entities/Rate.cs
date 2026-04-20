namespace billing.Entities;

public class Rate
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public Decimal Price { get; set; }
    public bool IsActive { get; set; }
    public string? Note { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }

    public Guid OrgId { get; set; }
    public Org Org { get; set; }
}
