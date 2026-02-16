using billing.Entities;

namespace billing.Services;

public interface IJwtService
{
    string GenerateAccessToken(User user, IEnumerable<string>? perms);
    string GenerateRefreshToken(int length = 32);
}
