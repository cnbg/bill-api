using billing.Constants;
using billing.DTOs;
using billing.Extensions;
using billing.Filters;
using billing.Helpers;
using billing.Services;
using Microsoft.AspNetCore.Mvc;

namespace billing.APIs;

public static class UserEndpoints
{
    public static RouteGroupBuilder MapUserEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("v1/user")
            .WithExceptionHandler(new HttpExceptionFilter())
            .WithTags("user");

        group.MapGet("list", async (
                IUserService userService,
                MyDataSourceLoadOptions loadOptions
            ) => await userService.GetUserListAsync(loadOptions))
            .RequirePermission(Permissions.UserView)
            .WithName("GetUserList");

        group.MapGet("show/{id:guid}", async (
                Guid id,
                IUserService userService
            ) => await userService.GetUserByIdAsync(id))
            .RequirePermission(Permissions.UserView)
            .WithName("GetUserById");

        group.MapPost("create", async (
                IUserService userService,
                [FromBody] CreateUserRequest createUserDto
            ) => await userService.CreateUserAsync(createUserDto))
            .RequirePermission(Permissions.UserCreate)
            .WithValidation<CreateUserRequest>()
            .WithName("CreateUser");

        // Update and Delete endpoints can be added similarly
        group.MapPut("update/{id:guid}", async (
                Guid id,
                IUserService userService,
                [FromBody] UpdateUserRequest updateUserDto
            ) =>
            {
                await userService.UpdateUserAsync(id, updateUserDto);
                return Results.NoContent();
            })
            .RequirePermission(Permissions.UserEdit)
            .WithValidation<UpdateUserRequest>()
            .WithName("UpdateUser");

        group.MapDelete("delete/{id:guid}", async (
                Guid id,
                IUserService userService
            ) =>
            {
                await userService.DeleteUserAsync(id);
                return Results.NoContent();
            })
            .RequirePermission(Permissions.UserDelete)
            .WithName("DeleteUser");

        return group;
    }
}
