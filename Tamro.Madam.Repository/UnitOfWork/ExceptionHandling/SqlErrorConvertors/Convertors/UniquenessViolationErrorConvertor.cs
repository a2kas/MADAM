using System.Text.RegularExpressions;
using Tamro.Madam.Repository.Common;

namespace Tamro.Madam.Repository.UnitOfWork.ExceptionHandling.SqlErrorConvertors.Convertors;
public static class UniquenessViolationErrorConvertor
{
    private static Regex FaultyKeyMatcher = new Regex(@"The duplicate key value is \((.*)\)");
    private static Regex LastPartOfIndexNameMatcher = new Regex(@"with unique index '.*_([^_^']*)'");

    public static string? Convert<EntityT>(
        string exceptionMessage,
        string entityDisplayName)
        where EntityT : class
    {
        var dupedValueMatch = FaultyKeyMatcher.Match(exceptionMessage);
        var indexLastNamePartMatch = LastPartOfIndexNameMatcher.Match(exceptionMessage);

        if (dupedValueMatch.Success && indexLastNamePartMatch.Success)
        {
            var duplicatedValue = dupedValueMatch.Groups[1].Value;
            var columnName = indexLastNamePartMatch.Groups[1].Value;

            var propertyDisplayName = columnName.GetPropertyDisplayNameOrFallback<EntityT>();

            return $@"{entityDisplayName} with {propertyDisplayName} '{duplicatedValue}' already exists.";
        }
        return null;
    }
}
