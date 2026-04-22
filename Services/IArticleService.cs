using billing.DTOs;
using billing.Helpers;
using DevExtreme.AspNet.Data.ResponseModel;

namespace billing.Services;

public interface IArticleService
{
    Task<LoadResult> GetArticleListAsync(MyDataSourceLoadOptions loadOptions);
    Task<ArticleDto> GetArticleByIdAsync(Guid id);
    Task<ArticleDto> CreateArticleAsync(CreateArticleRequest request);
    Task UpdateArticleAsync(Guid id, UpdateArticleRequest request);
    Task DeleteArticleAsync(Guid id);
}
