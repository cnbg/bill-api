namespace billing.DTOs;

public record RegionResponse(Guid Id, string Code, string Name);

public record CreateRegionRequest(string Code, string Name);

public record UpdateRegionRequest(string? Code, string? Name);
