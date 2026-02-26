namespace billing.DTOs;

public record AuthUser(
    Guid Id,
    string? Email,
    string Name,
    string Type,
    string Locale,
    string Theme
)
{
    public AuthUserOrg? Org { get; set; }
    public List<AuthUserOrg>? Orgs { get; set; }
    public List<AuthUserRole> Roles { get; set; } = [];
    public List<string> Perms { get; set; } = [];
}

public record AuthUserOrg(Guid Id, string Name);

public record AuthUserRole(Guid Id, string Name);

public record LoginRequest(string Username, string Password);

public record TokenResponse(string AccessToken, string RefreshToken, AuthUser User);

public record RefreshTokenRequest(string RefreshToken);

public record UpdateProfileRequest(string? Locale, string? Theme, Guid? OrgId);
