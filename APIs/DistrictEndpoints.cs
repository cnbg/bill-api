using billing.Constants;
using billing.DTOs;
using billing.Extensions;
using billing.Filters;
using billing.Helpers;
using billing.Services;
using Microsoft.AspNetCore.Mvc;

namespace billing.APIs;

public static class DistrictEndpoints
{
    public static RouteGroupBuilder MapDistrictEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("v1/district")
            .WithExceptionHandler(new HttpExceptionFilter())
            .WithTags("district");

        group.MapGet("list", async (
                IDistrictService districtService,
                MyDataSourceLoadOptions loadOptions
            ) => await districtService.GetDistrictListAsync(loadOptions))
            .RequirePermission(Permissions.DistrictView)
            .WithName("GetDistrictList");

        group.MapGet("show/{id:guid}", async (
                Guid id,
                IDistrictService districtService
            ) => await districtService.GetDistrictByIdAsync(id))
            .RequirePermission(Permissions.DistrictView)
            .WithName("GetDistrictById");

        group.MapPost("create", async (
                IDistrictService districtService,
                [FromBody] CreateDistrictRequest createDistrictDto
            ) => await districtService.CreateDistrictAsync(createDistrictDto))
            .RequirePermission(Permissions.DistrictCreate)
            .WithValidation<CreateDistrictRequest>()
            .WithName("CreateDistrict");

        // Update and Delete endpoints can be added similarly
        group.MapPut("update/{id:guid}", async (
                Guid id,
                IDistrictService districtService,
                [FromBody] UpdateDistrictRequest updateDistrictDto
            ) =>
            {
                await districtService.UpdateDistrictAsync(id, updateDistrictDto);
                return Results.NoContent();
            })
            .RequirePermission(Permissions.DistrictEdit)
            .WithValidation<UpdateDistrictRequest>()
            .WithName("UpdateDistrict");

        group.MapDelete("delete/{id:guid}", async (
                Guid id,
                IDistrictService districtService
            ) =>
            {
                await districtService.DeleteDistrictAsync(id);
                return Results.NoContent();
            })
            .RequirePermission(Permissions.DistrictDelete)
            .WithName("DeleteDistrict");

        return group;
    }
}
