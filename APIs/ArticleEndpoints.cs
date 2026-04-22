using billing.Constants;
using billing.DTOs;
using billing.Extensions;
using billing.Filters;
using billing.Helpers;
using billing.Services;
using Microsoft.AspNetCore.Mvc;

namespace billing.APIs;

public static class ArticleEndpoints
{
    public static RouteGroupBuilder MapArticleEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("v1/article")
            .WithExceptionHandler(new HttpExceptionFilter())
            .WithTags("article");

        group.MapGet("list", async (
                IArticleService articleService,
                MyDataSourceLoadOptions loadOptions
            ) => await articleService.GetArticleListAsync(loadOptions))
            .RequirePermission(Permissions.ArticleView)
            .WithName("GetArticleList");

        group.MapGet("show/{id:guid}", async (
                Guid id,
                IArticleService articleService
            ) => await articleService.GetArticleByIdAsync(id))
            .RequirePermission(Permissions.ArticleView)
            .WithName("GetArticleById");

        group.MapPost("create", async (
                IArticleService articleService,
                [FromBody] CreateArticleRequest createArticleDto
            ) => await articleService.CreateArticleAsync(createArticleDto))
            .RequirePermission(Permissions.ArticleCreate)
            .WithValidation<CreateArticleRequest>()
            .WithName("CreateArticle");

        // Update and Delete endpoints can be added similarly
        group.MapPut("update/{id:guid}", async (
                Guid id,
                IArticleService articleService,
                [FromBody] UpdateArticleRequest updateArticleDto
            ) =>
            {
                await articleService.UpdateArticleAsync(id, updateArticleDto);
                return Results.NoContent();
            })
            .RequirePermission(Permissions.ArticleEdit)
            .WithValidation<UpdateArticleRequest>()
            .WithName("UpdateArticle");

        group.MapDelete("delete/{id:guid}", async (
                Guid id,
                IArticleService articleService
            ) =>
            {
                await articleService.DeleteArticleAsync(id);
                return Results.NoContent();
            })
            .RequirePermission(Permissions.ArticleDelete)
            .WithName("DeleteArticle");

        return group;
    }
}
