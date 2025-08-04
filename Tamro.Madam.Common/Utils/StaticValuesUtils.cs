using System.Reflection;

namespace Tamro.Madam.Common.Utils;
public class StaticValuesUtils
{
    public static IEnumerable<string> GetStaticStringValuesDefinedOnType(Type constantsType)
    {
        return constantsType.GetFields(BindingFlags.Public | BindingFlags.Static)
                            .Where(field => field.FieldType == typeof(string))
                            .Select(field => (string?)field!.GetValue(null) ?? string.Empty);
    }
}
