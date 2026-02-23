namespace billing.DTOs;

public record DistrictResponse(Guid Id, string Code, string Name);

public record CreateDistrictRequest(string Code, string Name);

public record UpdateDistrictRequest(string? Code, string? Name);
