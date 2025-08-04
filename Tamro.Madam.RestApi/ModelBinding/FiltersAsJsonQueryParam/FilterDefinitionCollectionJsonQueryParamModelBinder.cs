using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Json;
using Tamro.Madam.Application.Models.Common;

namespace Tamro.Madam.RestApi.ModelBinding.FiltersAsJsonQueryParam;

public class FilterDefinitionCollectionJsonQueryParamModelBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        var modelName = bindingContext.ModelName;

        var baseValueProviderResult = bindingContext.ValueProvider.GetValue(modelName);

        if (baseValueProviderResult == ValueProviderResult.None)
        {
            bindingContext.Result = ModelBindingResult.Failed();
            return Task.CompletedTask;
        }

        var rawJson = baseValueProviderResult.FirstValue;

        if (string.IsNullOrWhiteSpace(rawJson))
        {
            bindingContext.Result = ModelBindingResult.Failed();
            return Task.CompletedTask;
        }

        try
        {
            var filters = JsonSerializer.Deserialize<List<FilterDefinition>>(
                rawJson,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    AllowTrailingCommas = true
                });

            bindingContext.Result = ModelBindingResult.Success(filters);
        }
        catch (JsonException ex)
        {
            bindingContext.ModelState.AddModelError("filters",
                $"Invalid JSON in 'filters' - the Filters argument must contain a json array with the filters definitions, or be absent.");
            bindingContext.Result = ModelBindingResult.Failed();
        }

        return Task.CompletedTask;
    }
}