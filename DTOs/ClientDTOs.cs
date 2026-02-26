namespace billing.DTOs;

public record ClientListDto(
    Guid Id,
    string Account,
    string Type,
    int Entrance,
    int Floor,
    string HouseNum,
    string ApartNum,
    float Area,
    int MembersCount,
    string Address,
    string Note
);

public record ClientDto(
    Guid Id,
    string Account,
    string Type,
    int Entrance,
    int Floor,
    string HouseNum,
    string ApartNum,
    float Area,
    int MembersCount,
    string Address,
    string Note
);

public record ClientResponse(Guid Id, string Account);

public record CreateClientRequest(string Account);

public record UpdateClientRequest(string? Account);
