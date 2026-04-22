namespace billing.Entities;

public class Balance
{
    public Guid Id { get; set; }
    public int Year { get; set; }
    public int Month { get; set; }
    public Decimal Charges { get; set; } = 0;
    public Decimal Payments { get; set; } = 0;
    public Decimal Fines { get; set; } = 0;
    public Decimal Start { get; set; }  = 0;
    public Decimal End { get; set; } = 0;

    public Guid? OrgId { get; set; }
    public Org? Org { get; set; }
}
