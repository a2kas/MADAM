using Ardalis.Specification;
using System.Linq.Expressions;
using System.Reflection;
using Tamro.Madam.Common.Constants.SearchExpressionConstants;

namespace Tamro.Madam.Application.Extensions.Specification;

public static class StringSpecificationExtensions
{
    public static void ApplyStringFilter<T>(this ISpecificationBuilder<T> builder, string filterOperator, string filterValue, Expression<Func<T, string>> propertySelector)
        where T : class
    {
        if (string.IsNullOrEmpty(filterOperator))
        {
            return;
        }

        switch (filterOperator)
        {
            case SearchStringConstants.Contains:
                builder.Where(CreateContainsExpression(propertySelector, filterValue));
                break;
            case SearchStringConstants.Equals:
                builder.Where(CreateEqualsExpression(propertySelector, filterValue));
                break;
            case SearchStringConstants.NotEquals:
                builder.Where(CreateNotEqualsExpression(propertySelector, filterValue));
                break;
            case SearchStringConstants.StartsWith:
                builder.Where(CreateStartsWithExpression(propertySelector, filterValue));
                break;
            case SearchStringConstants.EndsWith:
                builder.Where(CreateEndsWithExpression(propertySelector, filterValue));
                break;
            case SearchStringConstants.NotContains:
                builder.Where(CreateNotContainsExpression(propertySelector, filterValue));
                break;
            case SearchStringConstants.IsEmpty:
                builder.Where(CreateIsEmptyExpression(propertySelector));
                break;
            case SearchStringConstants.IsNotEmpty:
                builder.Where(CreateIsNotEmptyExpression(propertySelector));
                break;
        }
    }

    private static Expression<Func<T, bool>> CreateStartsWithExpression<T>(Expression<Func<T, string>> propertySelector, string value)
    {
        var method = typeof(string).GetMethod("StartsWith", [typeof(string)]);
        return CreateExpression(method, propertySelector, value);
    }

    private static Expression<Func<T, bool>> CreateEndsWithExpression<T>(Expression<Func<T, string>> propertySelector, string value)
    {
        var method = typeof(string).GetMethod("EndsWith", [typeof(string)]);
        return CreateExpression(method, propertySelector, value);
    }

    private static Expression<Func<T, bool>> CreateContainsExpression<T>(Expression<Func<T, string>> propertySelector, string value)
    {
        var method = typeof(string).GetMethod("Contains", [typeof(string)]);
        return CreateExpression(method, propertySelector, value);
    }

    private static Expression<Func<T, bool>> CreateNotContainsExpression<T>(Expression<Func<T, string>> propertySelector, string value)
    {
        var method = typeof(string).GetMethod("Contains", [typeof(string)]);
        return CreateExpression(method, propertySelector, value, notExpression: true);
    }

    private static Expression<Func<T, bool>> CreateEqualsExpression<T>(Expression<Func<T, string>> propertySelector, string value)
    {
        var method = typeof(string).GetMethod("Equals", [typeof(string)]);
        return CreateExpression(method, propertySelector, value);
    }

    private static Expression<Func<T, bool>> CreateNotEqualsExpression<T>(Expression<Func<T, string>> propertySelector, string value)
    {
        var method = typeof(string).GetMethod("Equals", [typeof(string)]);
        return CreateExpression(method, propertySelector, value, notExpression: true);
    }

    private static Expression<Func<T, bool>> CreateIsEmptyExpression<T>(Expression<Func<T, string>> propertySelector)
    {
        var method = typeof(string).GetMethod("IsNullOrEmpty", [typeof(string)]);
        return CreateExpressionWithoutValue(method, propertySelector);
    }

    private static Expression<Func<T, bool>> CreateIsNotEmptyExpression<T>(Expression<Func<T, string>> propertySelector)
    {
        var method = typeof(string).GetMethod("IsNullOrEmpty", [typeof(string)]);
        return CreateExpressionWithoutValue(method, propertySelector, notExpression: true);
    }

    private static Expression<Func<T, bool>> CreateExpression<T>(MethodInfo method, Expression<Func<T, string>> propertySelector, string value, bool notExpression = false)
    {
        if (string.IsNullOrEmpty(value))
        {
            return _ => true;
        }

        var parameterExp = propertySelector.Parameters[0];
        var propertyExp = propertySelector.Body;
        var valueExp = Expression.Constant(value, typeof(string));

        var expression = Expression.Call(propertyExp, method, valueExp);
        if (notExpression)
        {
            return Expression.Lambda<Func<T, bool>>(Expression.Not(expression), parameterExp);
        }

        return Expression.Lambda<Func<T, bool>>(expression, parameterExp);
    }

    private static Expression<Func<T, bool>> CreateExpressionWithoutValue<T>(MethodInfo method, Expression<Func<T, string>> propertySelector, bool notExpression = false)
    {
        var parameterExp = propertySelector.Parameters[0];
        var propertyExp = propertySelector.Body;

        var expression = Expression.Call(method, propertyExp);       
        if (notExpression)
        {
            return Expression.Lambda<Func<T, bool>>(Expression.Not(expression), parameterExp);
        }
        return Expression.Lambda<Func<T, bool>>(expression, parameterExp); 
    }
}
