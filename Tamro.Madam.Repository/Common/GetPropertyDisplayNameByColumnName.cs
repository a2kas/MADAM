using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace Tamro.Madam.Repository.Common;
public static class GetPropertyDisplayNameByColumnName
{
    public static string GetPropertyDisplayNameOrFallback<EntityT>(this string columnName)
    {
        var entityType = typeof(EntityT);

        PropertyInfo? propertyInfo = null;

        foreach (var property in entityType.GetProperties())
        {
            var columnAttribute = property.GetCustomAttribute<ColumnAttribute>();
            if (columnAttribute != null && columnAttribute.Name == columnName)
            {
                propertyInfo = property;
                break;
            }

            if (property.Name == columnName)
            {
                propertyInfo = property;
                break;
            }
        }

        if (propertyInfo != null) {
            var displayNameAttribute = propertyInfo.GetCustomAttribute<DisplayNameAttribute>();
            if (displayNameAttribute != null)
            {
                return displayNameAttribute.DisplayName;
            }
        }

        return columnName;
    }
}
