using billing.Constants;
using billing.DTOs;
using billing.Extensions;
using billing.Filters;
using billing.Helpers;
using billing.Services;
using Microsoft.AspNetCore.Mvc;

namespace billing.APIs;

public static class ChargeEndpoints
{
    public static RouteGroupBuilder MapChargeEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("v1/charge")
            .WithExceptionHandler(new HttpExceptionFilter())
            .WithTags("charge");

        group.MapGet("list", async (
                IChargeService chargeService,
                MyDataSourceLoadOptions loadOptions
            ) => await chargeService.GetChargeListAsync(loadOptions))
            .RequirePermission(Permissions.ChargeView)
            .WithName("GetChargeList");

        group.MapGet("show/{id:guid}", async (
                Guid id,
                IChargeService chargeService
            ) => await chargeService.GetChargeByIdAsync(id))
            .RequirePermission(Permissions.ChargeView)
            .WithName("GetChargeById");

        group.MapPost("create", async (
                IChargeService chargeService,
                [FromBody] CreateChargeRequest createChargeDto
            ) => await chargeService.CreateChargeAsync(createChargeDto))
            .RequirePermission(Permissions.ChargeCreate)
            .WithValidation<CreateChargeRequest>()
            .WithName("CreateCharge");

        group.MapPut("update/{id:guid}", async (
                Guid id,
                IChargeService chargeService,
                [FromBody] UpdateChargeRequest updateChargeDto
            ) =>
            {
                await chargeService.UpdateChargeAsync(id, updateChargeDto);
                return Results.NoContent();
            })
            .RequirePermission(Permissions.ChargeEdit)
            .WithValidation<UpdateChargeRequest>()
            .WithName("UpdateCharge");

        group.MapDelete("delete/{id:guid}", async (
                IChargeService chargeService,
                Guid id
            ) =>
            {
                await chargeService.DeleteChargeAsync(id);
                return Results.NoContent();
            })
            .RequirePermission(Permissions.ChargeDelete)
            .WithName("DeleteCharge");

        return group;
    }
}
