using Ardalis.Specification;
using System.Linq.Expressions;

namespace Tamro.Madam.Application.Extensions.Specification;

public static class BoolSpecificationExtensions
{
    public static void ApplyBoolFilter<T>(this ISpecificationBuilder<T> builder, bool? filterValue, Expression<Func<T, bool>> propertySelector)
        where T : class
    {
        if (filterValue != null)
        {
            builder.Where(CreateEqualsExpression(propertySelector, (bool)filterValue));
        }
    }

    private static Expression<Func<T, bool>> CreateEqualsExpression<T>(Expression<Func<T, bool>> propertySelector, bool value)
    {
        var parameterExp = propertySelector.Parameters[0];
        var propertyExp = propertySelector.Body;
        var valueExp = Expression.Constant(value, typeof(bool));

        var equalsMethod = typeof(bool).GetMethod("Equals", new[] { typeof(bool) });
        var equalsExp = Expression.Call(propertyExp, equalsMethod, valueExp);

        return Expression.Lambda<Func<T, bool>>(equalsExp, parameterExp);
    }
}
