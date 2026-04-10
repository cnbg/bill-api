namespace billing.DTOs;

public record RateDto(Guid Id, string Name);
public record CreateRateRequest(string Name);

public record UpdateRateRequest(string? Name);
