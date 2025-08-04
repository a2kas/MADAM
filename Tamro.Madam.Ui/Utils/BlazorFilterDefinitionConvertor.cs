using MudBlazor;
using Tamro.Madam.Application.Models.Common;

namespace Tamro.Madam.Ui.Utils;

public static class BlazorFilterDefinitionConvertor
{
    public static FilterDefinition[] ConvertToApplicationLevelFilterDefinitions<T>(this ICollection<IFilterDefinition<T>> blazorFilterDefinitions)
    {
        return blazorFilterDefinitions.Select(f => new FilterDefinition
        {
            Column = f.Column!.PropertyName,
            Operator = f.Operator!,
            Value = f.Value!,
        }).ToArray();
    }
}
