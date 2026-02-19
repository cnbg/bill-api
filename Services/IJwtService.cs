using billing.DTOs;

namespace billing.Services;

public interface IJwtService
{
    string GenerateAccessToken(AuthUser user);
    string GenerateRefreshToken(int length = 32);
}
