using System.Text.Json.Serialization;

namespace billing.Entities;

public class Client
{
    public Guid Id { get; set; }
    public string Account { get; set; } = string.Empty;
    public int Entrance { get; set; }
    public int Floor { get; set; }
    public string HouseNum { get; set; }
    public string ApartNum { get; set; }
    public float Area { get; set; }
    public int MembersCount { get; set; }
    public string Address { get; set; }
    public string? Note { get; set; }

    public Guid ClientTypeId { get; set; }
    public ClientType ClientType { get; set; }
}
