namespace billing.Helpers;

public class ExceptionMap
{
    private readonly Dictionary<Type, Func<Exception, IResult>> _handlers = new();
    private Func<Exception, IResult> _default = _ => Results.InternalServerError(new { error = "An unexpected error occurred." });

    public ExceptionMap Map<TException>(Func<TException, IResult> handler)
        where TException : Exception
    {
        _handlers[typeof(TException)] = ex => handler((TException)ex);
        return this;
    }

    public ExceptionMap Default(Func<Exception, IResult> handler)
    {
        _default = handler;
        return this;
    }

    public IResult Handle(Exception ex)
    {
        var type = ex.GetType();

        while (type != null)
        {
            if (_handlers.TryGetValue(type, out var handler))
                return handler(ex);

            type = type.BaseType;
        }

        return _default(ex);
    }
}
