namespace billing.DTOs;

public record ClientTypeDto(Guid Id, string Name, bool? IsActive);

public record CreateClientTypeRequest(string Name, bool? IsActive);

public record UpdateClientTypeRequest(string? Name, bool? IsActive);
