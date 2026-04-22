namespace billing.DTOs;

public record BalanceDto(Guid Id, int Year, int Month, Decimal Charges, Decimal Payments, Decimal Fines, Decimal Start, Decimal End, Guid? OrgId);

public record CreateBalanceRequest(int Year, int Month, Decimal Charges, Decimal Payments, Decimal Fines, Decimal Start, Decimal End, Guid? OrgId);

public record UpdateBalanceRequest(int? Year, int? Month, Decimal Charges, Decimal Payments, Decimal Fines, Decimal Start, Decimal End, Guid? OrgId);
