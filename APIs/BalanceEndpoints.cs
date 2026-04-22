using billing.Constants;
using billing.DTOs;
using billing.Extensions;
using billing.Filters;
using billing.Helpers;
using billing.Services;
using Microsoft.AspNetCore.Mvc;

namespace billing.APIs;

public static class BalanceEndpoints
{
    public static RouteGroupBuilder MapBalanceEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("v1/Balance")
            .WithExceptionHandler(new HttpExceptionFilter())
            .WithTags("Balance");

        group.MapGet("list", async (
                IBalanceService balanceService,
                MyDataSourceLoadOptions loadOptions
            ) => await balanceService.GetBalanceListAsync(loadOptions))
            .RequirePermission(Permissions.BalanceView)
            .WithName("GetBalanceList");

        group.MapGet("show/{id:guid}", async (
                Guid id,
                IBalanceService balanceService
            ) => await balanceService.GetBalanceByIdAsync(id))
            .RequirePermission(Permissions.BalanceView)
            .WithName("GetBalanceById");

        group.MapPost("create", async (
                IBalanceService balanceService,
                [FromBody] CreateBalanceRequest createBalanceDto
            ) => await balanceService.CreateBalanceAsync(createBalanceDto))
            .RequirePermission(Permissions.BalanceCreate)
            .WithValidation<CreateBalanceRequest>()
            .WithName("CreateBalance");

        group.MapPut("update/{id:guid}", async (
                Guid id,
                IBalanceService balanceService,
                [FromBody] UpdateBalanceRequest updateBalanceDto
            ) =>
            {
                await balanceService.UpdateBalanceAsync(id, updateBalanceDto);
                return Results.NoContent();
            })
            .RequirePermission(Permissions.BalanceEdit)
            .WithValidation<UpdateBalanceRequest>()
            .WithName("UpdateBalance");

        group.MapDelete("delete/{id:guid}", async (
                IBalanceService balanceService,
                Guid id
            ) =>
            {
                await balanceService.DeleteBalanceAsync(id);
                return Results.NoContent();
            })
            .RequirePermission(Permissions.BalanceDelete)
            .WithName("DeleteBalance");

        return group;
    }
}
