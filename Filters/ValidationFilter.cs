using FluentValidation;

namespace billing.Filters;

public class ValidationFilter<T>(IValidator<T> validator) : IEndpointFilter
    where T : class
{
    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext ctx,
        EndpointFilterDelegate next)
    {
        var arg = ctx.Arguments.OfType<T>().FirstOrDefault();

        if (arg is null)
            return Results.BadRequest(new { error = "Invalid request body." });

        var result = await validator.ValidateAsync(arg);

        if (!result.IsValid)
            return Results.UnprocessableEntity(new
            {
                errors = result.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray())
            });

        return await next(ctx);
    }
}
