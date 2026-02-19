using billing.DTOs;

namespace billing.Services;

public interface IAuthService
{
    Task<TokenResponse> LoginAsync(LoginRequest request);
    Task<TokenResponse> RefreshTokenAsync(RefreshTokenRequest request);
    Task RevokeTokenAsync();
    Task<AuthUser> GetAuthUserAsync();
    Task<TokenResponse> UpdateAuthProfileAsync(UpdateProfileRequest request);
}
