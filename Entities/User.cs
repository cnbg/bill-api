namespace billing.Entities;

public class User
{
    public Guid Id { get; set; }
    public bool IsActive { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? MidName { get; set; }
    public string? Email { get; set; }
    public string? PassHash { get; set; }
    public string Locale { get; set; } = "ru";
    public string Theme { get; set; } = "light";
    public string Type { get; set; } = "user";
    public string? RefreshToken { get; set; }
    public DateTime? RefreshExp { get; set; }

    public Guid? OrgId { get; set; }
    public Org? Org { get; set; }
}
