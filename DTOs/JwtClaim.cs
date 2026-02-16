namespace billing.DTOs;

public record JwtClaim(Guid UserId, Guid OrgId, string Type, string Locale, DateTime ExpiresAt, string Issuer, string Audience);
