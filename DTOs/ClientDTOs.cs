namespace billing.DTOs;

public record ClientResponse(Guid Id, string Account);

public record CreateClientRequest(string Account);

public record UpdateClientRequest(string? Account);
