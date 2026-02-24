namespace billing.DTOs;

public record ChargeDto(Guid Id, string Account, decimal Amount, string? Status, int Year, int Month, string? Note, Guid? OrgId);

public record CreateChargeRequest(string Account, decimal Amount, string Status, int Year, int Month, string? Note, Guid? OrgId);

public record UpdateChargeRequest(string? Account, decimal? Amount, string? Status, int? Year, int? Month, string? Note, Guid? OrgId);
