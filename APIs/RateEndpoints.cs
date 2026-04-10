using billing.Constants;
using billing.DTOs;
using billing.Extensions;
using billing.Filters;
using billing.Helpers;
using billing.Services;
using Microsoft.AspNetCore.Mvc;

namespace billing.APIs;

public static class RateEndpoints
{
    public static RouteGroupBuilder MapRateEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("v1/rate")
            .WithExceptionHandler(new HttpExceptionFilter())
            .WithTags("rate");

        group.MapGet("list", async (
                IRateService rateService,
                MyDataSourceLoadOptions loadOptions
            ) => await rateService.GetRateListAsync(loadOptions))
            .RequirePermission(Permissions.RateView)
            .WithName("GetRateList");

        group.MapGet("show/{id:guid}", async (
                Guid id,
                IRateService rateService
            ) => await rateService.GetRateByIdAsync(id))
            .RequirePermission(Permissions.RateView)
            .WithName("GetRateById");

        group.MapPost("create", async (
                IRateService rateService,
                [FromBody] CreateRateRequest createRateDto
            ) => await rateService.CreateRateAsync(createRateDto))
            .RequirePermission(Permissions.RateCreate)
            .WithValidation<CreateRateRequest>()
            .WithName("CreateRate");

        group.MapPut("update/{id:guid}", async (
                Guid id,
                IRateService rateService,
                [FromBody] UpdateRateRequest updateRateDto
            ) =>
            {
                await rateService.UpdateRateAsync(id, updateRateDto);
                return Results.NoContent();
            })
            .RequirePermission(Permissions.RateEdit)
            .WithValidation<UpdateRateRequest>()
            .WithName("UpdateRate");

        group.MapDelete("delete/{id:guid}", async (
                IRateService rateService,
                Guid id
            ) =>
            {
                await rateService.DeleteRateAsync(id);
                return Results.NoContent();
            })
            .RequirePermission(Permissions.RateDelete)
            .WithName("DeleteRate");

        return group;
    }
}
