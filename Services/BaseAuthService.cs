using billing.DTOs;
using billing.Extensions;

namespace billing.Services;

public class BaseAuthService(IHttpContextAccessor ctxAccessor)
{
    protected readonly JwtClaim JwtDto = (ctxAccessor.HttpContext ?? throw new InvalidOperationException()).GetJwtClaims();
}
