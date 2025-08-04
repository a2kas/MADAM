using Ardalis.Specification;
using System.Linq.Expressions;
using Tamro.Madam.Common.Constants.SearchExpressionConstants;

namespace Tamro.Madam.Application.Extensions.Specification;

public static class DateTimeSpecificationExtensions
{
    public static void ApplyDateTimeFilter<T>(this ISpecificationBuilder<T> builder, string filterOperator, DateTime? filterValue, Expression<Func<T, DateTime?>> propertySelector)
        where T : class
    {
        if (string.IsNullOrEmpty(filterOperator))
        {
            return;
        }
        if (filterValue == null && filterOperator != SearchDateTimeConstants.IsEmpty && filterOperator != SearchDateTimeConstants.IsNotEmpty)
        {
            return;
        }

        switch (filterOperator)
        {
            case SearchDateTimeConstants.Is:
                builder.Where(CreateIsSameAsExpression(propertySelector, filterValue));
                break;
            case SearchDateTimeConstants.IsNot:
                builder.Where(CreateIsNotSameAsExpression(propertySelector, filterValue));
                break;
            case SearchDateTimeConstants.IsAfter:
                builder.Where(CreateIsAfterExpression(propertySelector, filterValue));
                break;
            case SearchDateTimeConstants.IsOnOrAfter:
                builder.Where(CreateIsOnOrAfterExpression(propertySelector, filterValue));
                break;
            case SearchDateTimeConstants.IsBefore:
                builder.Where(CreateIsBeforeExpression(propertySelector, filterValue));
                break;
            case SearchDateTimeConstants.IsOnOrBefore:
                builder.Where(CreateIsOnOrBeforeExpression(propertySelector, filterValue));
                break;
            case SearchDateTimeConstants.IsEmpty:
                builder.Where(CreateIsEmptyExpression(propertySelector));
                break;
            case SearchDateTimeConstants.IsNotEmpty:
                builder.Where(CreateIsNotEmptyExpression(propertySelector));
                break;
        }
    }

    private static Expression<Func<T, bool>> CreateIsSameAsExpression<T>(Expression<Func<T, DateTime?>> propertySelector, DateTime? value)
    {
        var parameterExp = propertySelector.Parameters[0];
        var propertyExp = propertySelector.Body;

        DateTime toValue;
        if (value.Value.Hour == default && value.Value.Minute == default)
        {
            toValue = value.Value.AddDays(1);
        }
        else
        {
            toValue = value.Value.AddMinutes(1);
        }

        var greaterThanOrEqualExp = Expression.GreaterThanOrEqual(propertyExp, Expression.Constant(value, typeof(DateTime?)));
        var lessThanExp = Expression.LessThan(propertyExp, Expression.Constant(toValue, typeof(DateTime?)));

        var combinedExp = Expression.AndAlso(greaterThanOrEqualExp, lessThanExp);

        return Expression.Lambda<Func<T, bool>>(combinedExp, parameterExp);
    }

    private static Expression<Func<T, bool>> CreateIsNotSameAsExpression<T>(Expression<Func<T, DateTime?>> propertySelector, DateTime? value)
    {
        var parameterExp = propertySelector.Parameters[0];
        var propertyExp = propertySelector.Body;

        DateTime toValue;
        if (value.Value.Hour == default && value.Value.Minute == default)
        {
            toValue = value.Value.AddDays(1);
        }
        else
        {
            toValue = value.Value.AddMinutes(1);
        }

        var greaterThanOrEqualExp = Expression.GreaterThanOrEqual(propertyExp, Expression.Constant(toValue, typeof(DateTime?)));
        var lessThanExp = Expression.LessThan(propertyExp, Expression.Constant(value, typeof(DateTime?)));

        var combinedExp = Expression.Or(greaterThanOrEqualExp, lessThanExp);

        return Expression.Lambda<Func<T, bool>>(combinedExp, parameterExp);
    }

    private static Expression<Func<T, bool>> CreateIsAfterExpression<T>(Expression<Func<T, DateTime?>> propertySelector, DateTime? value)
    {
        var parameterExp = propertySelector.Parameters[0];
        var propertyExp = propertySelector.Body;

        var isAfterExp = Expression.GreaterThan(propertyExp, Expression.Constant(value, typeof(DateTime?)));
        return Expression.Lambda<Func<T, bool>>(isAfterExp, parameterExp);
    }

    private static Expression<Func<T, bool>> CreateIsOnOrAfterExpression<T>(Expression<Func<T, DateTime?>> propertySelector, DateTime? value)
    {
        var parameterExp = propertySelector.Parameters[0];
        var propertyExp = propertySelector.Body;

        var isOnOrAfterExp = Expression.GreaterThanOrEqual(propertyExp, Expression.Constant(value, typeof(DateTime?)));

        return Expression.Lambda<Func<T, bool>>(isOnOrAfterExp, parameterExp);
    }

    private static Expression<Func<T, bool>> CreateIsOnOrBeforeExpression<T>(Expression<Func<T, DateTime?>> propertySelector, DateTime? value)
    {
        var parameterExp = propertySelector.Parameters[0];
        var propertyExp = propertySelector.Body;

        var isOnOrBeforeExp = Expression.LessThanOrEqual(propertyExp, Expression.Constant(value, typeof(DateTime?)));

        return Expression.Lambda<Func<T, bool>>(isOnOrBeforeExp, parameterExp);
    }

    private static Expression<Func<T, bool>> CreateIsBeforeExpression<T>(Expression<Func<T, DateTime?>> propertySelector, DateTime? value)
    {
        var parameterExp = propertySelector.Parameters[0];
        var propertyExp = propertySelector.Body;

        var isBeforeExp = Expression.LessThan(propertyExp, Expression.Constant(value, typeof(DateTime?)));

        return Expression.Lambda<Func<T, bool>>(isBeforeExp, parameterExp);
    }

    private static Expression<Func<T, bool>> CreateIsEmptyExpression<T>(Expression<Func<T, DateTime?>> propertySelector)
    {
        var parameterExp = propertySelector.Parameters[0];
        var propertyExp = propertySelector.Body;

        var isNullExp = Expression.Equal(propertyExp, Expression.Constant(null, typeof(DateTime?)));

        return Expression.Lambda<Func<T, bool>>(isNullExp, parameterExp);
    }

    private static Expression<Func<T, bool>> CreateIsNotEmptyExpression<T>(Expression<Func<T, DateTime?>> propertySelector)
    {
        var parameterExp = propertySelector.Parameters[0];
        var propertyExp = propertySelector.Body;

        var isNotNullExp = Expression.NotEqual(propertyExp, Expression.Constant(null, typeof(DateTime?)));

        return Expression.Lambda<Func<T, bool>>(isNotNullExp, parameterExp);
    }
}
