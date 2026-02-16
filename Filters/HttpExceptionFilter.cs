using System.ComponentModel.DataAnnotations;
using billing.Helpers;

namespace billing.Filters;

public class HttpExceptionFilter : IEndpointFilter
{
    private readonly ExceptionMap _exceptionMap;

    public HttpExceptionFilter()
    {
        _exceptionMap = new ExceptionMap()
            .Map<UnauthorizedAccessException>(ex => Results.Unauthorized())
            .Map<ArgumentException>(ex => Results.BadRequest(new { error = ex.Message }))
            .Map<KeyNotFoundException>(ex => Results.NotFound(new { error = ex.Message }))
            .Map<ValidationException>(ex => Results.UnprocessableEntity(new { error = ex.Message }))
            .Default(ex => Results.Problem(detail: ex.Message, statusCode: 500));
    }

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        try
        {
            return await next(context);
        }
        catch (Exception ex)
        {
            return _exceptionMap.Handle(ex);
        }
    }
}
