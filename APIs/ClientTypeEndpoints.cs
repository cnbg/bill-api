using billing.Constants;
using billing.DTOs;
using billing.Extensions;
using billing.Filters;
using billing.Helpers;
using billing.Services;
using Microsoft.AspNetCore.Mvc;

namespace billing.APIs;

public static class ClientTypeEndpoints
{
    public static RouteGroupBuilder MapClientTypeEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("v1/clientType")
            .WithExceptionHandler(new HttpExceptionFilter())
            .WithTags("client-type");

        group.MapGet("list", async (
                IClientTypeService clientTypeService,
                MyDataSourceLoadOptions loadOptions
            ) => await clientTypeService.GetClientTypeListAsync(loadOptions))
            .RequirePermission(Permissions.ClientTypeView)
            .WithName("GetClientTypeList");

        group.MapGet("show/{id:guid}", async (
                Guid id,
                IClientTypeService clientTypeService
            ) => await clientTypeService.GetClientTypeByIdAsync(id))
            .RequirePermission(Permissions.ClientTypeView)
            .WithName("GetClientTypeById");

        group.MapPost("create", async (
                IClientTypeService clientTypeService,
                [FromBody] CreateClientTypeRequest createClientTypeDto
            ) => await clientTypeService.CreateClientTypeAsync(createClientTypeDto))
            .RequirePermission(Permissions.ClientTypeCreate)
            .WithValidation<CreateClientTypeRequest>()
            .WithName("CreateClientType");

        // Update and Delete endpoints can be added similarly
        group.MapPut("update/{id:guid}", async (
                Guid id,
                IClientTypeService clientTypeService,
                [FromBody] UpdateClientTypeRequest updateClientTypeDto
            ) =>
            {
                await clientTypeService.UpdateClientTypeAsync(id, updateClientTypeDto);
                return Results.NoContent();
            })
            .RequirePermission(Permissions.ClientTypeEdit)
            .WithValidation<UpdateClientTypeRequest>()
            .WithName("UpdateClientType");

        group.MapDelete("delete/{id:guid}", async (
                Guid id,
                IClientTypeService clientTypeService
            ) =>
            {
                await clientTypeService.DeleteClientTypeAsync(id);
                return Results.NoContent();
            })
            .RequirePermission(Permissions.ClientTypeDelete)
            .WithName("DeleteClientType");

        return group;
    }
}
