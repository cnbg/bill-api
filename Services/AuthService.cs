using billing.DTOs;
using billing.Entities;
using billing.Extensions;
using Microsoft.EntityFrameworkCore;

namespace billing.Services;

public class AuthService(
    AppDbCtx dbCtx,
    IJwtService jwtService,
    IConfiguration conf,
    IHttpContextAccessor ctxAccessor
) : IAuthService
{
    private readonly JwtClaim _jwtDto = (ctxAccessor.HttpContext ?? throw new InvalidOperationException()).GetJwtClaims();

    public async Task<TokenResponse> LoginAsync(LoginRequest request)
    {
        var user = await dbCtx.Users
            .Include(u => u.Org)
            .FirstOrDefaultAsync(u => u.Email == request.Username);

        if (user is not { IsActive: true } || !BCrypt.Net.BCrypt.Verify(request.Password, user.PassHash))
            throw new ArgumentException("Invalid username or password");

        return await TokenResponse(user, true);
    }

    public async Task<TokenResponse> RefreshTokenAsync(RefreshTokenRequest request)
    {
        var user = await dbCtx.Users
            .Include(u => u.Org)
            .FirstOrDefaultAsync(u => u.RefreshToken == request.RefreshToken && u.RefreshExp > DateTime.UtcNow);

        if (user is not { IsActive: true })
            throw new ArgumentException("Invalid refresh token");

        return await TokenResponse(user, true);
    }

    public async Task RevokeTokenAsync()
    {
        var user = await dbCtx.Users.FirstOrDefaultAsync(u => u.Id == _jwtDto.UserId);

        if (user is not { IsActive: true })
            throw new ArgumentException("User not found");

        user.RefreshToken = null;
        user.RefreshExp = null;

        await dbCtx.SaveChangesAsync();
    }

    public async Task<AuthUser> GetAuthUserAsync()
    {
        var user = await dbCtx.Users
            .Include(u => u.Org)
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == _jwtDto.UserId);

        return user is not { IsActive: true } ? throw new ArgumentException("User not found") : BuildUserDto(user);
    }

    public async Task UpdateAuthProfileAsync(UpdateProfileRequest request)
    {
        var user = await dbCtx.Users.FirstOrDefaultAsync(u => u.Id == _jwtDto.UserId);

        if (user is not { IsActive: true })
            throw new ArgumentException("User not found");

        user.Theme = request.Theme ?? user.Theme;
        user.Locale = request.Locale ?? user.Locale;

        await dbCtx.SaveChangesAsync();
    }

    private static AuthUser BuildUserDto(User user)
    {
        return new AuthUser(
            user.Id,
            user.Email,
            user.FirstName + " " + user.LastName,
            user.OrgId,
            user.Org?.Name,
            user.Locale,
            user.Theme
        );
    }

    private async Task<TokenResponse> TokenResponse(User user, bool refreshExp = false)
    {
        var authUser = BuildUserDto(user);

        var perms = new[] { "region-view", "region-create" };

        var accessToken = jwtService.GenerateAccessToken(user, perms);
        var refreshToken = jwtService.GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        if (refreshExp)
            user.RefreshExp = DateTime.UtcNow.AddDays(Convert.ToDouble(conf["Jwt:RefreshTokenExpDays"]));

        await dbCtx.SaveChangesAsync();

        return new TokenResponse(
            accessToken,
            refreshToken,
            authUser
        );
    }
}
