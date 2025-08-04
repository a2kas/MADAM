using Tamro.Madam.Common.Constants.SearchExpressionConstants;

namespace Tamro.Madam.Common.Utils;
public static class DotnetToJsTypeConversion
{
    public static (string jsTypeName, IEnumerable<string> operators) GetJsTypeAndValidOperatorsFor(Type clrType)
    {
        var type = Nullable.GetUnderlyingType(clrType) ?? clrType;

        // we operate on enums in the string form and not as integers, so this is a special case
        if (type.IsEnum)
        {
            return ("string", StaticValuesUtils.GetStaticStringValuesDefinedOnType(typeof(SearchStringConstants)));
        }

        switch (Type.GetTypeCode(type))
        {
            case TypeCode.Boolean:
                return ("string", StaticValuesUtils.GetStaticStringValuesDefinedOnType(typeof(SearchBoolConstants)));

            case TypeCode.String:
                return ("string", StaticValuesUtils.GetStaticStringValuesDefinedOnType(typeof(SearchStringConstants)));

            case TypeCode.Int32:
            case TypeCode.Int64:
            case TypeCode.Decimal:
            case TypeCode.Double:
            case TypeCode.Single:
                return ("number", StaticValuesUtils.GetStaticStringValuesDefinedOnType(typeof(SearchNumberConstants)));

            case TypeCode.DateTime:
                return ("string (ISO 8601 date)", StaticValuesUtils.GetStaticStringValuesDefinedOnType(typeof(SearchDateTimeConstants)));

            default:
                throw new SystemException($@"Failed to generate swagger doc for paginated data endpoint - could not select a set of Operators for type {clrType.FullName}");
        }
    }
}
