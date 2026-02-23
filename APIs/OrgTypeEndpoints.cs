using billing.Constants;
using billing.DTOs;
using billing.Extensions;
using billing.Filters;
using billing.Helpers;
using billing.Services;
using Microsoft.AspNetCore.Mvc;

namespace billing.APIs;

public static class OrgTypeEndpoints
{
    public static RouteGroupBuilder MapOrgTypeEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("v1/orgType")
            .WithExceptionHandler(new HttpExceptionFilter())
            .WithTags("orgType");

        group.MapGet("list", async (
                IOrgTypeService orgTypeService,
                MyDataSourceLoadOptions loadOptions
            ) => await orgTypeService.GetOrgTypeListAsync(loadOptions))
            .RequirePermission(Permissions.OrgTypeView)
            .WithName("GetOrgTypeList");

        group.MapGet("show/{id:guid}", async (
                Guid id,
                IOrgTypeService orgTypeService
            ) => await orgTypeService.GetOrgTypeByIdAsync(id))
            .RequirePermission(Permissions.OrgTypeView)
            .WithName("GetOrgTypeById");

        group.MapPost("create", async (
                IOrgTypeService orgTypeService,
                [FromBody] CreateOrgTypeRequest createOrgTypeDto
            ) => await orgTypeService.CreateOrgTypeAsync(createOrgTypeDto))
            .RequirePermission(Permissions.OrgTypeCreate)
            .WithValidation<CreateOrgTypeRequest>()
            .WithName("CreateOrgType");

        // Update and Delete endpoints can be added similarly
        group.MapPut("update/{id:guid}", async (
                Guid id,
                IOrgTypeService orgTypeService,
                [FromBody] UpdateOrgTypeRequest updateOrgTypeDto
            ) =>
            {
                await orgTypeService.UpdateOrgTypeAsync(id, updateOrgTypeDto);
                return Results.NoContent();
            })
            .RequirePermission(Permissions.OrgTypeEdit)
            .WithValidation<UpdateOrgTypeRequest>()
            .WithName("UpdateOrgType");

        group.MapDelete("delete/{id:guid}", async (
                Guid id,
                IOrgTypeService orgTypeService
            ) =>
            {
                await orgTypeService.DeleteOrgTypeAsync(id);
                return Results.NoContent();
            })
            .RequirePermission(Permissions.OrgTypeDelete)
            .WithName("DeleteOrgType");

        return group;
    }
}
