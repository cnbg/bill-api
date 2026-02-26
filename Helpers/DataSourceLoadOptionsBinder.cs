using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace billing.Helpers;

[ModelBinder(BinderType = typeof(DataSourceLoadOptionsBinder))]
public class DataSourceLoadOptions : DataSourceLoadOptionsBase {}

 public class DataSourceLoadOptionsBinder : IModelBinder {
    public Task BindModelAsync(ModelBindingContext bindingContext) {
        var loadOptions = new DataSourceLoadOptions();
        loadOptions.StringToLower ??= true;
        DataSourceLoadOptionsParser.Parse(loadOptions, key => bindingContext.ValueProvider.GetValue(key).FirstOrDefault());
        bindingContext.Result = ModelBindingResult.Success(loadOptions);
        return Task.CompletedTask;
    }
}

public class MyDataSourceLoadOptions : DataSourceLoadOptionsBase
{
    public static ValueTask<MyDataSourceLoadOptions> BindAsync(HttpContext httpContext)
    {
        var loadOptions = new MyDataSourceLoadOptions();
        loadOptions.StringToLower ??= true;
        // Use DataSourceLoadOptionsParser to populate the loadOptions from the query string
        DataSourceLoadOptionsParser.Parse(loadOptions, key => httpContext.Request.Query.TryGetValue(key, out var value) ? value.FirstOrDefault() : null);
        return ValueTask.FromResult(loadOptions);
    }
}
