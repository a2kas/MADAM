namespace Tamro.Madam.Common.Regex;

public static class NumericRegex
{
    public static readonly string PositiveDecimal = @"[0-9.]";
    public static readonly string PositiveInteger = @"[0-9]";
    public static readonly string Integer = @"[0-9-]";
    public static readonly string Decimal = @"[0-9.-]";
}
