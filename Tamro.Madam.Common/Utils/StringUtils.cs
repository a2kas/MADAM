namespace Tamro.Madam.Common.Utils;

public static class StringUtils
{
    public static string ConvertCamelCaseToSentence(string input)
    {
        string spacedString = System.Text.RegularExpressions.Regex.Replace(input, "(?<!^)([A-Z])", " $1");

        return char.ToUpper(spacedString[0]) + spacedString[1..].ToLower();
    }
}
