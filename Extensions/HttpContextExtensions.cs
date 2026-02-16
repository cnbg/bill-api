using billing.DTOs;

namespace billing.Extensions;

public static class HttpContextExtensions
{
    extension(HttpContext ctx)
    {
        public JwtClaim GetJwtClaims()
        {
            return new JwtClaim(
                Guid.TryParse(ctx.User.FindFirst("usr")?.Value, out var userId) ? userId : Guid.Empty,
                Guid.TryParse(ctx.User.FindFirst("org")?.Value, out var orgId) ? orgId : Guid.Empty,
                ctx.User.FindFirst("typ")?.Value ?? string.Empty,
                ctx.User.FindFirst("loc")?.Value ?? string.Empty,
                DateTime.TryParse(ctx.User.FindFirst("exp")?.Value, out var expAt) ? expAt : DateTime.MinValue,
                ctx.User.FindFirst("iss")?.Value ?? string.Empty,
                ctx.User.FindFirst("aud")?.Value ?? string.Empty
            );
        }
    }
}
