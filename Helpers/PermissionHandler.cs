using billing.Constants;
using billing.Providers;
using Microsoft.AspNetCore.Authorization;

namespace billing.Helpers;

public sealed class PermissionHandler : AuthorizationHandler<PermissionRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        // Must be authenticated
        if (context.User.Identity?.IsAuthenticated != true)
            return Task.CompletedTask;

        var hasPermission = context.User.Claims.Any(c =>
            c.Type == Permissions.ClaimType &&
            string.Equals(c.Value, requirement.Permission, StringComparison.OrdinalIgnoreCase));

        if (hasPermission)
            context.Succeed(requirement);

        return Task.CompletedTask;
    }
}
