namespace billing.DTOs;

public record DistrictResponse(Guid Id, Guid RegionId, string Code, string Name);

public record CreateDistrictRequest(Guid RegionId, string Code, string Name);

public record UpdateDistrictRequest(Guid? RegionId, string? Code, string? Name);
