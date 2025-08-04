using System.ComponentModel;
using System.Linq.Expressions;

namespace Tamro.Madam.Common.Utils;

public static class DisplayNameHelper
{
    public static string Get<T>(Expression<Func<T>> expression)
    {
        var memberExpression = (MemberExpression)expression.Body;
        var displayAttribute = (DisplayNameAttribute)Attribute.GetCustomAttribute(
            memberExpression.Member,
            typeof(DisplayNameAttribute)
        );

        return displayAttribute?.DisplayName ?? memberExpression.Member.Name;
    }

    public static string Get(Type type, string propertyName)
    {
        var property = type.GetProperty(propertyName);

        if (property != null)
        {
            var displayAttribute = (DisplayNameAttribute)Attribute.GetCustomAttribute(
                property,
                typeof(DisplayNameAttribute)
            );

            return displayAttribute?.DisplayName ?? property.Name;
        }

        return propertyName;
    }
}
