using System.ComponentModel;
using System.Reflection;
using Microsoft.Data.SqlClient;
using Tamro.Madam.Repository.Entities;
using Tamro.Madam.Repository.UnitOfWork.ExceptionHandling.SqlErrorConvertors.Convertors;

namespace Tamro.Madam.Repository.UnitOfWork.ExceptionHandling.SqlErrorConvertors;
public static class SqlErrorConvertor
{
    public static string ConvertToUserFriendlyMessage<EntityT>(this SqlException exception, string defaultMessage) where EntityT : class, IEntity
    {
        var entityDisplayName = typeof(EntityT).GetCustomAttribute<DisplayNameAttribute>()?.DisplayName;
        if (entityDisplayName == null)
        {
            entityDisplayName = typeof(EntityT).Name;
        }

        if (exception.Message.StartsWith("Cannot insert duplicate key row"))
        {
            return UniquenessViolationErrorConvertor.Convert<EntityT>(exception.Message, entityDisplayName)
                ?? defaultMessage;
        }

        // ... handle and early-return other kinds of sql errors as need appears

        return defaultMessage;
    }
}
