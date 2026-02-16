using billing.Constants;
using billing.DTOs;
using billing.Extensions;
using billing.Filters;
using billing.Helpers;
using billing.Services;
using Microsoft.AspNetCore.Mvc;

namespace billing.APIs;

public static class RegionEndpoints
{
    public static RouteGroupBuilder MapRegionEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("v1/region")
            .WithExceptionHandler(new HttpExceptionFilter())
            .WithTags("region");

        group.MapGet("list", async (
                IRegionService regionService,
                MyDataSourceLoadOptions loadOptions
            ) => await regionService.GetRegionListAsync(loadOptions))
            // .RequireAuthorization("Authenticated")
            .WithName("GetRegionList");

        group.MapGet("show/{id:guid}", async (
                Guid id,
                IRegionService regionService
            ) => await regionService.GetRegionByIdAsync(id))
            // .RequireAuthorization("Authenticated")
            .RequirePermission(Permissions.RegionView)
            .WithName("GetRegionById");

        group.MapPost("create", async (
                [FromBody] CreateRegionRequest createRegionDto,
                IRegionService regionService
            ) => await regionService.CreateRegionAsync(createRegionDto))
            // .RequireAuthorization("Authenticated")
            .WithValidation<CreateRegionRequest>()
            .WithName("CreateRegion");

        // Update and Delete endpoints can be added similarly
        group.MapPut("update/{id:guid}", async (
                Guid id,
                [FromBody] UpdateRegionRequest updateRegionDto,
                IRegionService regionService
            ) =>
            {
                await regionService.UpdateRegionAsync(id, updateRegionDto);
                return Results.NoContent();
            })
            // .RequireAuthorization("Authenticated")
            .WithValidation<UpdateRegionRequest>()
            .WithName("UpdateRegion");

        group.MapDelete("delete/{id:guid}", async (
                Guid id,
                IRegionService regionService
            ) =>
            {
                await regionService.DeleteRegionAsync(id);
                return Results.NoContent();
            })
            // .RequireAuthorization("Authenticated")
            .WithName("DeleteRegion");

        return group;
    }
}
