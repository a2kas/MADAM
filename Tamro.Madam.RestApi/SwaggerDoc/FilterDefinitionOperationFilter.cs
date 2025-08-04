using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using System.Text;
using Tamro.Madam.Common.Utils;
using Tamro.Madam.Repository.Common;
using Tamro.Madam.RestApi.Contracts.SwaggerDoc;

namespace Tamro.Madam.Common.SwaggerDoc;

/// <summary>
/// Add documentation on endpoints which return PaginatedData,
/// telling which fields are filterable and what operators can be appled to them for filtering.
/// </summary>
public class FilterDefinitionOperationFilter : IOperationFilter
{
    private readonly ILogger<FilterDefinitionOperationFilter> _logger;

    public FilterDefinitionOperationFilter(ILogger<FilterDefinitionOperationFilter> logger)
        => _logger = logger;

    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (!context.ApiDescription.TryGetMethodInfo(out var methodInfo))
        {
            throw new SystemException(
                $"Failed to find MethodInfo for {context.ApiDescription.ActionDescriptor.DisplayName} " +
                $"of {context.ApiDescription.GroupName}");
        }

        var swaggerResponseAttr = methodInfo
            .GetCustomAttributes(true)
            .OfType<SwaggerResponseAttribute>()
            .FirstOrDefault(a => a.StatusCode == 200 && a.Type != null);
        var responseType = swaggerResponseAttr?.Type;

        if (responseType == null)
        {
            _logger.LogError(
                "No response type found on '{MethodName}' of '{TypeName}'",
                methodInfo.Name, methodInfo.DeclaringType?.Name);
            return;
        }

        if (!responseType.IsGenericType ||
            responseType.GetGenericTypeDefinition() != typeof(PaginatedData<>))
        {
            return;
        }

        var itemType = responseType.GetGenericArguments()[0];

        var filterableProps = itemType
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.CanRead &&
                        p.GetCustomAttribute<DocumentAsFilterableAttribute>() != null)
            .ToList();

        if (!filterableProps.Any())
        { // on some Grid endpoints, there is no flexible filters served, and that's ok
          // - no additional description to be generated in such case
            return;
        }

        StringBuilder sb = BuildFiltersInfoMarkdownTable(operation, itemType, filterableProps);
        operation.Description = sb.ToString();
    }

    private static StringBuilder BuildFiltersInfoMarkdownTable(OpenApiOperation operation, Type itemType, List<PropertyInfo> filterableProps)
    {
        var sb = new StringBuilder(operation.Description ?? string.Empty);

        sb.AppendLine($@"Here’s how to structure the `Filters` array in the request body when filtering `{itemType.Name}` entries:");

        sb.AppendLine("| Field | Type | Valid Operators | Example Filter |");
        sb.AppendLine("|-------|------|-----------------|----------------|");

        foreach (var prop in filterableProps)
        {
            var (jsType, operators) = DotnetToJsTypeConversion.GetJsTypeAndValidOperatorsFor(prop.PropertyType);
            var operatorsString = string.Join(", ", operators);
            var exampleFilter = $"{{ \"Column\": \"{prop.Name}\", \"Operator\": \"{operators.First()}\", \"Value\": \"\" }}";

            sb.AppendLine($"| `{prop.Name}` | `{jsType}` | {operatorsString} | `{exampleFilter}` |");
        }
        sb.AppendLine();
        sb.AppendLine("`Filters` field ***has to be a single array of objects*** - the filtering objects described above, e.g.:");
        sb.AppendLine("``` js");
        sb.AppendLine("[ {...}, {...} ]");
        sb.AppendLine("```");

        return sb;
    }
}
