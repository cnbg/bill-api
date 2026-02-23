namespace billing.Entities;

public class District
{
    public Guid Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;

    public Guid RegionId { get; set; }
    public Region Region { get; set; }
}
