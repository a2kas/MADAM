using Microsoft.AspNetCore.Mvc.ModelBinding;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.RestApi.ModelBinding.FiltersAsJsonQueryParam;

namespace Tamro.Madam.RestApi.ModelBinding;

public class FilterDefinitionCollectionJsonModelBinderProvider : IModelBinderProvider
{
    public IModelBinder GetBinder(ModelBinderProviderContext context)
    {
        var isCollectionOfFilterDef = typeof(IEnumerable<FilterDefinition>).IsAssignableFrom(context.Metadata.ModelType);
        if (isCollectionOfFilterDef)
        {
            return new FilterDefinitionCollectionJsonQueryParamModelBinder();
        }

        return null;
    }
}