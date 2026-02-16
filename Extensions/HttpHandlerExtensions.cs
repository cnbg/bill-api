using billing.Filters;

namespace billing.Extensions;

public static class HttpHandlerExtensions
{
    extension<TBuilder>(TBuilder builder) where TBuilder : IEndpointConventionBuilder
    {
        public TBuilder WithExceptionHandler(HttpExceptionFilter exFilter)
        {
            return builder.AddEndpointFilter(exFilter);
        }
    }

    // Apply to an entire group
    public static RouteGroupBuilder WithExceptionHandler(this RouteGroupBuilder group, HttpExceptionFilter exFilter)
    {
        return group.AddEndpointFilter(exFilter);
    }
}
