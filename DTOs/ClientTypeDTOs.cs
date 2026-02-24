namespace billing.DTOs;

public record ClientTypeDto(Guid Id, string Name);

public record CreateClientTypeRequest(string Name);

public record UpdateClientTypeRequest(string? Name);
