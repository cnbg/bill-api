namespace billing.DTOs;

public record PaymentDto(Guid Id, string Account, decimal Amount, string Source, string Status, int Year, int Month, string? Note, Guid? OrgId);

public record CreatePaymentRequest(string Account, decimal Amount, string Source, string Status, int Year, int Month, string? Note, Guid? OrgId);

public record UpdatePaymentRequest(string? Account, decimal? Amount, string? Source, string? Status, int? Year, int? Month, string? Note, Guid? OrgId);
