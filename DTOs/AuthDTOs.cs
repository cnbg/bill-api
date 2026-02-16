namespace billing.DTOs;

public record AuthUser(Guid Id, string? Email, string Name, Guid? OrgId, string? OrgName, string Locale, string Theme);

public record LoginRequest(string Username, string Password);

public record TokenResponse(string AccessToken, string RefreshToken, AuthUser User);

public record RefreshTokenRequest(string RefreshToken);

public record UpdateProfileRequest(string? Locale, string? Theme);
