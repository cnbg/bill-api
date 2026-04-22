namespace billing.DTOs;

public record ArticleDto(Guid Id, string Name, string? Content, Guid? OrgId);

public record CreateArticleRequest(string Name, string? Content, Guid? OrgId);

public record UpdateArticleRequest(string? Name, string? Content, Guid? OrgId);
