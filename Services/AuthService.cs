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

        if (user is not { IsActive: true })
            throw new ArgumentException("User not found");

        return await BuildUserDto(user);
    }

    public async Task<TokenResponse> UpdateAuthProfileAsync(UpdateProfileRequest request)
    {
        var user = await dbCtx.Users
            .Include(u => u.Org)
            .FirstOrDefaultAsync(u => u.Id == _jwtDto.UserId);

        if (user is not { IsActive: true })
            throw new ArgumentException("User not found");

        user.Theme = request.Theme ?? user.Theme;
        user.Locale = request.Locale ?? user.Locale;

        // check user can switch to the new org
        if (request.OrgId != null && request.OrgId != user.OrgId)
        {
            var hasAccessToOrg = await dbCtx.UserRole
                .Include(ur => ur.Org)
                .AnyAsync(ur => ur.UserId == user.Id && ur.OrgId == request.OrgId && ur.Org.IsActive);

            if (!hasAccessToOrg)
                throw new ArgumentException("User does not have access to the specified organization");

            user.OrgId = request.OrgId;
        }

        await dbCtx.SaveChangesAsync();

        return await TokenResponse(user, true);
    }

    private async Task<TokenResponse> TokenResponse(User user, bool refreshExp = false)
    {
        var authUser = await BuildUserDto(user);

        var accessToken = jwtService.GenerateAccessToken(authUser);
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

    private async Task<AuthUser> BuildUserDto(User user)
    {
        var authUser = new AuthUser(
            user.Id,
            user.Email,
            $"{user.FirstName} {user.LastName}".Trim(),
            user.Type,
            user.Locale,
            user.Theme
        );

        var roles = await GetRoles(user);
        authUser.Roles = roles.Select(ur => ur.Role.Name).ToList();

        var orgs = await GetOrgs(user);
        authUser.Orgs = orgs.Select(ur => new AuthUserOrg(ur.Org.Id, ur.Org.Name)).ToList();

        // set to user org_id
        if (user.OrgId != null)
        {
            var org = authUser.Orgs?.FirstOrDefault(o => o.Id == user.OrgId);
            if (org != null)
                authUser.Org = org;
        }

        authUser.Perms = roles.SelectMany(ur => ur.Role.Perms).Distinct().ToList();

        return authUser;
    }

    private async Task<List<UserRole>> GetRoles(User user)
    {
        if (user.OrgId == null) return [];

        return await dbCtx.UserRole
            .Include(ur => ur.Role)
            .Where(ur => ur.UserId == user.Id && ur.OrgId == user.OrgId && ur.Role.IsActive)
            .Distinct()
            .AsNoTracking()
            .ToListAsync();
    }

    private async Task<List<UserRole>> GetOrgs(User user)
    {
        if (user.OrgId == null) return [];

        return await dbCtx.UserRole
            .Include(ur => ur.Org)
            .Where(ur => ur.UserId == user.Id && ur.Org.IsActive)
            .GroupBy(ur => ur.OrgId)
            .Select(g => g.First())
            .AsNoTracking()
            .ToListAsync();
    }
}
