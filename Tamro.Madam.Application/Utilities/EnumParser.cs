namespace Tamro.Madam.Application.Utilities;
public static class EnumParser
{
    private const string _nullValue = "NULL";

    public static T? ParseNullable<T>(string? value) where T : struct
    {
        if (string.IsNullOrEmpty(value) || value.Equals(_nullValue, StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        if (Enum.TryParse<T>(value, out var parsedValue))
        {
            return parsedValue;
        }

        return null;
    }
}
