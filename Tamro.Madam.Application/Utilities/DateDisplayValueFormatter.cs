using System.Globalization;

namespace Tamro.Madam.Application.Utilities;
public static class DateDisplayValueFormatter
{
    /// <summary>
    /// UI helper utility the default value of an argument with a specified masked value or date format as string
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <param name="maskedValue"></param>
    /// <param name="dateFormat"></param>
    /// <returns>string</returns>
    public static string FormatOrDefault<T>(T value, string maskedValue = "", string dateFormat = "yyyy-MM-dd HH:mm:ss")
    {
        maskedValue ??= "";

        if (EqualityComparer<T>.Default.Equals(value, default(T)))
        {
            return maskedValue;
        }

        if (value is DateTime dateTimeValue && !string.IsNullOrEmpty(dateFormat))
        {
            return dateTimeValue.ToString(dateFormat, CultureInfo.InvariantCulture);
        }

        return value.ToString();
    }
}
