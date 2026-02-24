using billing.Constants;
using billing.DTOs;
using billing.Extensions;
using billing.Filters;
using billing.Helpers;
using billing.Services;
using Microsoft.AspNetCore.Mvc;

namespace billing.APIs;

public static class RoleEndpoints
{
    public static RouteGroupBuilder MapRoleEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("v1/role")
            .WithExceptionHandler(new HttpExceptionFilter())
            .WithTags("role");

        group.MapGet("list", async (
                IRoleService roleService,
                MyDataSourceLoadOptions loadOptions
            ) => await roleService.GetRoleListAsync(loadOptions))
            .RequirePermission(Permissions.RoleView)
            .WithName("GetRoleList");

        group.MapGet("show/{id:guid}", async (
                Guid id,
                IRoleService roleService
            ) => await roleService.GetRoleByIdAsync(id))
            .RequirePermission(Permissions.RoleView)
            .WithName("GetRoleById");

        group.MapPost("create", async (
                IRoleService roleService,
                [FromBody] CreateRoleRequest createRoleDto
            ) => await roleService.CreateRoleAsync(createRoleDto))
            .RequirePermission(Permissions.RoleCreate)
            .WithValidation<CreateRoleRequest>()
            .WithName("CreateRole");

        group.MapPut("update/{id:guid}", async (
                Guid id,
                IRoleService roleService,
                [FromBody] UpdateRoleRequest updateRoleDto
            ) => await roleService.UpdateRoleAsync(id, updateRoleDto))
            .RequirePermission(Permissions.RoleEdit)
            .WithValidation<UpdateRoleRequest>()
            .WithName("UpdateRole");

        group.MapPut("perm/{id:guid}", async (
                Guid id,
                IRoleService roleService,
                [FromBody] UpdateRolePermRequest request
            ) => await roleService.UpdateRolePermAsync(id, request))
            .RequirePermission(Permissions.RoleEdit)
            .WithValidation<UpdateRolePermRequest>()
            .WithName("ToggleRolePerm");

        group.MapDelete("delete/{id:guid}", async (
                Guid id,
                IRoleService roleService
            ) => await roleService.DeleteRoleAsync(id))
            .RequirePermission(Permissions.RoleDelete)
            .WithName("DeleteRole");

        return group;
    }
}
