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
            .RequirePermission(Permissions.RegionView)
            .WithName("GetRegionList");

        group.MapGet("show/{id:guid}", async (
                Guid id,
                IRegionService regionService
            ) => await regionService.GetRegionByIdAsync(id))
            .RequirePermission(Permissions.RegionView)
            .WithName("GetRegionById");

        group.MapPost("create", async (
                IRegionService regionService,
                [FromBody] CreateRegionRequest createRegionDto
            ) => await regionService.CreateRegionAsync(createRegionDto))
            .RequirePermission(Permissions.RegionCreate)
            .WithValidation<CreateRegionRequest>()
            .WithName("CreateRegion");

        // Update and Delete endpoints can be added similarly
        group.MapPut("update/{id:guid}", async (
                Guid id,
                IRegionService regionService,
                [FromBody] UpdateRegionRequest updateRegionDto
            ) =>
            {
                await regionService.UpdateRegionAsync(id, updateRegionDto);
                return Results.NoContent();
            })
            .RequirePermission(Permissions.RegionEdit)
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
            .RequirePermission(Permissions.RegionDelete)
            .WithName("DeleteRegion");

        return group;
    }
}
