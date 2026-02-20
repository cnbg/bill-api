namespace billing.Entities;

public class Payment
{
    public Guid Id { get; set; }
    public string Account { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Source { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public int Year { get; set; }
    public int Month { get; set; }
    public string? Note { get; set; }

    public Guid? OrgId { get; set; }
    public Org? Org { get; set; }
}
