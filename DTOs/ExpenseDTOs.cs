namespace billing.DTOs;

public record ExpenseDto(Guid Id, decimal Amount, string? Status, int Year, int Month, string? Note, Guid? OrgId);

public record CreateExpenseRequest(decimal Amount, string? Status, int Year, int Month, string? Note, Guid? OrgId);

public record UpdateExpenseRequest(decimal? Amount, string? Status, int? Year, int? Month, string? Note, Guid? OrgId);
