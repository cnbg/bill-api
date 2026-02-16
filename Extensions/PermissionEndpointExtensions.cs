namespace billing.Extensions;

public static class PermissionEndpointExtensions
{
    public static RouteHandlerBuilder RequirePermission(this RouteHandlerBuilder builder, string permission)
        => builder.RequireAuthorization($"perm:{permission}");
}
