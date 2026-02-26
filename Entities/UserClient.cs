namespace billing.Entities;

public class UserClient
{
    public Guid OrgId { get; set; }
    public Org Org { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; }

    public Guid ClientId { get; set; }
    public Client Client { get; set; }

    public string Note { get; set; } = string.Empty;
    public bool IsOwner { get; set; } = false;
}
