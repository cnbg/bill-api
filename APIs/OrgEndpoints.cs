using billing.Constants;
using billing.DTOs;
using billing.Extensions;
using billing.Filters;
using billing.Helpers;
using billing.Services;
using Microsoft.AspNetCore.Mvc;

namespace billing.APIs;

public static class OrgEndpoints
{
    public static RouteGroupBuilder MapOrgEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("v1/org")
            .WithExceptionHandler(new HttpExceptionFilter())
            .WithTags("org");

        group.MapGet("list", async (
                IOrgService orgService,
                MyDataSourceLoadOptions loadOptions
            ) => await orgService.GetOrgListAsync(loadOptions))
            .RequirePermission(Permissions.OrgView)
            .WithName("GetOrgList");

        group.MapGet("show/{id:guid}", async (
                Guid id,
                IOrgService orgService
            ) => await orgService.GetOrgByIdAsync(id))
            .RequirePermission(Permissions.OrgView)
            .WithName("GetOrgById");

        group.MapPost("create", async (
                IOrgService orgService,
                [FromBody] CreateOrgRequest createOrgDto
            ) => await orgService.CreateOrgAsync(createOrgDto))
            .RequirePermission(Permissions.OrgCreate)
            .WithValidation<CreateOrgRequest>()
            .WithName("CreateOrg");

        group.MapPut("update/{id:guid}", async (
                Guid id,
                IOrgService orgService,
                [FromBody] UpdateOrgRequest updateOrgDto
            ) =>
            {
                await orgService.UpdateOrgAsync(id, updateOrgDto);
                return Results.NoContent();
            })
            .RequirePermission(Permissions.OrgEdit)
            .WithValidation<UpdateOrgRequest>()
            .WithName("UpdateOrg");

        group.MapDelete("delete/{id:guid}", async (
                Guid id,
                IOrgService orgService
            ) =>
            {
                await orgService.DeleteOrgAsync(id);
                return Results.NoContent();
            })
            .RequirePermission(Permissions.OrgDelete)
            .WithName("DeleteOrg");

        return group;
    }
}
