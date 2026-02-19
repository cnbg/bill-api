using billing.DTOs;
using billing.Extensions;
using billing.Filters;
using billing.Services;
using Microsoft.AspNetCore.Mvc;

namespace billing.APIs;

public static class AuthEndpoints
{
    public static RouteGroupBuilder MapAuthEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("v1/auth")
            .WithExceptionHandler(new HttpExceptionFilter())
            .WithTags("auth");

        group.MapPost("login", async (
                IAuthService authService,
                [FromBody] LoginRequest request
            ) => await authService.LoginAsync(request))
            .WithValidation<LoginRequest>()
            .WithName("Login");

        group.MapPost("refresh", async (
                IAuthService authService,
                [FromBody] RefreshTokenRequest request
            ) => await authService.RefreshTokenAsync(request))
            .WithValidation<RefreshTokenRequest>()
            .WithName("RefreshToken");

        group.MapPost("revoke", async (IAuthService authService) =>
            {
                await authService.RevokeTokenAsync();
                return Results.NoContent();
            })
            .RequireAuthorization()
            .WithName("RevokeToken");

        group.MapGet("profile", async (
                IAuthService authService
            ) => await authService.GetAuthUserAsync())
            .RequireAuthorization()
            .WithName("GetAuthProfile");

        group.MapPut("update", async (
                IAuthService authService,
                [FromBody] UpdateProfileRequest request
            ) => await authService.UpdateAuthProfileAsync(request))
            .RequireAuthorization()
            .WithValidation<UpdateProfileRequest>()
            .WithName("UpdateAuthProfile");

        return group;
    }
}
