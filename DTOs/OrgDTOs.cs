using billing.Entities;

namespace billing.DTOs;

public record OrgDto(Guid Id, Guid OrgTypeId, string Name, string? Inn, string? Okpo, Decimal Balance, string? Note, bool? IsActive);
public record CreateOrgRequest(Guid OrgTypeId, string Name, string? Inn, string? Okpo, Decimal Balance, string? Note, bool? IsActive);

public record UpdateOrgRequest(Guid? OrgTypeId, string? Name, string? Inn, string? Okpo, Decimal? Balance, string? Note, bool? IsActive);
