using Ardalis.Specification;
using System.Linq.Expressions;
using Tamro.Madam.Common.Constants.SearchExpressionConstants;

namespace Tamro.Madam.Application.Extensions.Specification;

public static class IntSpecificationExtensions
{
    public static void ApplyIntFilter<T>(this ISpecificationBuilder<T> builder, string filterOperator, object filterObject, Expression<Func<T, int>> propertySelector)
        where T : class
    {
        if (filterObject == null)
        {
            if (filterOperator == SearchNumberConstants.IsEmpty)
            {
                builder.Where(CreateIsEmptyExpression(propertySelector));
            }
            if (filterOperator == SearchNumberConstants.IsNotEmpty)
            {
                builder.Where(CreateIsNotEmptyExpression(propertySelector));
            }
            return;
        }

        int.TryParse(filterObject.ToString(), out var filterValue);
        
        switch (filterOperator)
        {
            case SearchNumberConstants.Equals:
                builder.Where(CreateEqualsExpression(propertySelector, filterValue));
                break;
            case SearchNumberConstants.NotEquals:
                builder.Where(CreateNotEqualsExpression(propertySelector, filterValue));
                break;
            case SearchNumberConstants.GreaterThan:
                builder.Where(CreateGreaterThanExpression(propertySelector, filterValue));
                break;
            case SearchNumberConstants.LessThan:
                builder.Where(CreateLessThanExpression(propertySelector, filterValue));
                break;
            case SearchNumberConstants.GreaterThanOrEqual:
                builder.Where(CreateGreaterThanOrEqualExpression(propertySelector, filterValue));
                break;
            case SearchNumberConstants.LessThanOrEqual:
                builder.Where(CreateLessThanOrEqualExpression(propertySelector, filterValue));
                break;
            case SearchNumberConstants.IsNotEmpty:
                builder.Where(CreateIsNotEmptyExpression(propertySelector));
                break;
            case SearchNumberConstants.IsEmpty:
                builder.Where(CreateIsEmptyExpression(propertySelector));
                break;
        }
    }

    private static Expression<Func<T, bool>> CreateEqualsExpression<T>(Expression<Func<T, int>> propertySelector, int value)
    {
        var parameterExp = propertySelector.Parameters[0];
        var propertyExp = propertySelector.Body;
        var valueExp = Expression.Constant(value, typeof(int));

        var equalsExp = Expression.Equal(propertyExp, valueExp);

        return Expression.Lambda<Func<T, bool>>(equalsExp, parameterExp);
    }

    private static Expression<Func<T, bool>> CreateNotEqualsExpression<T>(Expression<Func<T, int>> propertySelector, int value)
    {
        var parameterExp = propertySelector.Parameters[0];
        var propertyExp = propertySelector.Body;
        var valueExp = Expression.Constant(value, typeof(int));

        var notEqualsExp = Expression.NotEqual(propertyExp, valueExp);

        return Expression.Lambda<Func<T, bool>>(notEqualsExp, parameterExp);
    }

    private static Expression<Func<T, bool>> CreateGreaterThanExpression<T>(Expression<Func<T, int>> propertySelector, int value)
    {
        var parameterExp = propertySelector.Parameters[0];
        var propertyExp = propertySelector.Body;
        var valueExp = Expression.Constant(value, typeof(int));

        var greaterThanExp = Expression.GreaterThan(propertyExp, valueExp);

        return Expression.Lambda<Func<T, bool>>(greaterThanExp, parameterExp);
    }

    private static Expression<Func<T, bool>> CreateLessThanExpression<T>(Expression<Func<T, int>> propertySelector, int value)
    {
        var parameterExp = propertySelector.Parameters[0];
        var propertyExp = propertySelector.Body;
        var valueExp = Expression.Constant(value, typeof(int));

        var lessThanExp = Expression.LessThan(propertyExp, valueExp);

        return Expression.Lambda<Func<T, bool>>(lessThanExp, parameterExp);
    }

    private static Expression<Func<T, bool>> CreateLessThanOrEqualExpression<T>(Expression<Func<T, int>> propertySelector, int value)
    {
        var parameterExp = propertySelector.Parameters[0];
        var propertyExp = propertySelector.Body;
        var valueExp = Expression.Constant(value, typeof(int));

        var lessThanOrEqualExp = Expression.LessThanOrEqual(propertyExp, valueExp);

        return Expression.Lambda<Func<T, bool>>(lessThanOrEqualExp, parameterExp);
    }

    private static Expression<Func<T, bool>> CreateGreaterThanOrEqualExpression<T>(Expression<Func<T, int>> propertySelector, int value)
    {
        var parameterExp = propertySelector.Parameters[0];
        var propertyExp = propertySelector.Body;
        var valueExp = Expression.Constant(value, typeof(int));

        var greaterThanOrEqualExp = Expression.GreaterThanOrEqual(propertyExp, valueExp);

        return Expression.Lambda<Func<T, bool>>(greaterThanOrEqualExp, parameterExp);
    }

    private static Expression<Func<T, bool>> CreateIsEmptyExpression<T>(Expression<Func<T, int>> propertySelector)
    {
        var parameterExp = propertySelector.Parameters[0];
        var propertyExp = propertySelector.Body;

        var isNullExp = Expression.Equal(propertyExp, Expression.Constant(0, typeof(int)));

        return Expression.Lambda<Func<T, bool>>(isNullExp, parameterExp);
    }

    private static Expression<Func<T, bool>> CreateIsNotEmptyExpression<T>(Expression<Func<T, int>> propertySelector)
    {
        var parameterExp = propertySelector.Parameters[0];
        var propertyExp = propertySelector.Body;

        var isNullExp = Expression.NotEqual(propertyExp, Expression.Constant(0, typeof(int)));

        return Expression.Lambda<Func<T, bool>>(isNullExp, parameterExp);
    }
}
