using billing.Constants;
using billing.DTOs;
using billing.Extensions;
using billing.Filters;
using billing.Helpers;
using billing.Services;
using Microsoft.AspNetCore.Mvc;

namespace billing.APIs;

public static class ClientEndpoints
{
    public static RouteGroupBuilder MapClientEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("v1/client")
            .WithExceptionHandler(new HttpExceptionFilter())
            .WithTags("client");

        group.MapGet("list", async (
                IClientService clientService,
                MyDataSourceLoadOptions loadOptions
            ) => await clientService.GetClientListAsync(loadOptions))
            .RequirePermission(Permissions.ClientView)
            .WithName("GetClientList");

        group.MapGet("show/{id:guid}", async (
                Guid id,
                IClientService clientService
            ) => await clientService.GetClientByIdAsync(id))
            .RequirePermission(Permissions.ClientView)
            .WithName("GetClientById");

        group.MapPost("create", async (
                IClientService clientService,
                [FromBody] CreateClientRequest createClientDto
            ) => await clientService.CreateClientAsync(createClientDto))
            .RequirePermission(Permissions.ClientCreate)
            .WithValidation<CreateClientRequest>()
            .WithName("CreateClient");

        // Update and Delete endpoints can be added similarly
        group.MapPut("update/{id:guid}", async (
                Guid id,
                IClientService clientService,
                [FromBody] UpdateClientRequest updateClientDto
            ) =>
            {
                await clientService.UpdateClientAsync(id, updateClientDto);
                return Results.NoContent();
            })
            .RequirePermission(Permissions.ClientEdit)
            .WithValidation<UpdateClientRequest>()
            .WithName("UpdateClient");

        group.MapDelete("delete/{id:guid}", async (
                Guid id,
                IClientService clientService
            ) =>
            {
                await clientService.DeleteClientAsync(id);
                return Results.NoContent();
            })
            .RequirePermission(Permissions.ClientDelete)
            .WithName("DeleteClient");

        return group;
    }
}
