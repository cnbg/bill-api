using billing.DTOs;
using billing.Entities;
using billing.Extensions;
using billing.Helpers;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.EntityFrameworkCore;

namespace billing.Services;

public class ArticleService(AppDbCtx dbCtx, IHttpContextAccessor ctxAccessor) : BaseAuthService(ctxAccessor), IArticleService
{
    public async Task<LoadResult> GetArticleListAsync(MyDataSourceLoadOptions loadOptions)
    {
        var query = dbCtx.Articles
            .Include(p => p.Org)
            .Where(p => p.OrgId == JwtDto.OrgId)
            .AsNoTracking()
            .AsQueryable();

        return await DataSourceLoader.LoadAsync(query, loadOptions);
    }

    public async Task<ArticleDto> GetArticleByIdAsync(Guid id)
    {
        var article = await dbCtx.Articles
            .Include(p => p.Org)
            .Where(p => p.Id == id && p.OrgId == JwtDto.OrgId)
            .Select(p => new ArticleDto(
                p.Id,
                p.Name,
                p.Content,
                p.OrgId
            ))
            .AsNoTracking()
            .FirstOrDefaultAsync();

        return article ?? throw new KeyNotFoundException("Article not found");
    }

    public async Task<ArticleDto> CreateArticleAsync(CreateArticleRequest request)
    {
        var resp = dbCtx.Articles.Add(new Article
        {
            OrgId = JwtDto.OrgId,
            Name = request.Name,
            Content = request.Content ?? string.Empty
        });
        await dbCtx.SaveChangesAsync();

        return resp.Entity != null
            ? new ArticleDto(
                resp.Entity.Id,
                resp.Entity.Name,
                resp.Entity.Content,
                resp.Entity.OrgId
            )
            : throw new ArgumentException("Failed to create article");
    }

    public async Task UpdateArticleAsync(Guid id, UpdateArticleRequest request)
    {
        var article = await dbCtx.Articles
            .Where(p => p.Id == id && p.OrgId == JwtDto.OrgId)
            .FirstOrDefaultAsync();

        if (article == null)
            throw new KeyNotFoundException("Article not found");

        article.Name = request.Name ?? article.Name;
        article.Content = request.Content ?? article.Content;

        await dbCtx.SaveChangesAsync();
    }

    public async Task DeleteArticleAsync(Guid id)
    {
        var article = await dbCtx.Articles
            .Where(p => p.Id == id && p.OrgId == JwtDto.OrgId)
            .FirstOrDefaultAsync();

        if (article == null)
            throw new KeyNotFoundException("Article not found");

        dbCtx.Articles.Remove(article);
        await dbCtx.SaveChangesAsync();
    }
}
