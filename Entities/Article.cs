namespace billing.Entities;

public class Article
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;

    public Guid? OrgId { get; set; }
    public Org? Org { get; set; }
}
