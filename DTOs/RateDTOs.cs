namespace billing.DTOs;

public record RateDto(Guid Id, string Name, decimal Price, string Type, string? Note, DateOnly StartDate,  DateOnly? EndDate, bool IsActive);
public record CreateRateRequest(string Name, decimal Price, string Type, string? Note, string StartDate,  string? EndDate, bool? IsActive);

public record UpdateRateRequest(string? Name, decimal? Price, string? Type, string? Note, string? StartDate,  string? EndDate, bool? IsActive);
