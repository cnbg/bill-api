using billing.Constants;
using billing.DTOs;
using billing.Extensions;
using billing.Filters;
using billing.Helpers;
using billing.Services;
using Microsoft.AspNetCore.Mvc;

namespace billing.APIs;

public static class PermEndpoints
{
    public static RouteGroupBuilder MapPermEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("v1/perm")
            .WithExceptionHandler(new HttpExceptionFilter())
            .WithTags("perm");

        group.MapGet("list", async () => await Task.FromResult(Permissions.GetList()))
            .RequirePermission(Permissions.RoleView)
            .WithName("GetPermList");

        return group;
    }
}
