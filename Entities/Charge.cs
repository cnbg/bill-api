namespace billing.Entities;

public class Charge
{
    public Guid Id { get; set; }
    public string Account { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string? Status { get; set; }
    public int Year { get; set; }
    public int Month { get; set; }
    public string? Note { get; set; }

    public Guid? OrgId { get; set; }
    public Org? Org { get; set; }
}
