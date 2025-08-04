using System.Linq.Expressions;
using FluentValidation;
using NUnit.Framework;

namespace Tamro.Madam.Application.Tests.TestExtensions.Validation;
public static class IValidatorExtensions
{
    public static void ShouldBeValid<T>(this IValidator<T> validator, T model)
    {
        var result = validator.Validate(model);
        Assert.That(
            result.IsValid,
            $"Unexpected error(s) of validation: [{string.Join(";\n", result.Errors)}]");
    }

    public static void ShouldHaveValidationErrorFor<T, TProperty>(
        this IValidator<T> validator, T model, Expression<Func<T, TProperty>> propertyExpression)
    {
        ValidateProperty(validator, model, propertyExpression, expectError: true);
    }

    public static void ShouldNotHaveValidationErrorFor<T, TProperty>(
        this IValidator<T> validator, T model, Expression<Func<T, TProperty>> propertyExpression)
    {
        ValidateProperty(validator, model, propertyExpression, expectError: false);
    }

    private static void ValidateProperty<T, TProperty>(
        IValidator<T> validator, T model, Expression<Func<T, TProperty>> propertyExpression, bool expectError)
    {
        var propertyName = GetPropertyName(propertyExpression);
        var result = validator.Validate(model);
        var hasError = result.Errors.Any(x => x.PropertyName == propertyName);

        if (expectError)
        {
            Assert.That(hasError, $"Expected a validation error for property '{propertyName}', but none was found.");
        }
        else
        {
            Assert.That(!hasError, $"Expected no validation error for property '{propertyName}', but one was found.");
        }
    }

    private static string GetPropertyName<T, TProperty>(Expression<Func<T, TProperty>> propertyExpression)
    {
        if (propertyExpression.Body is MemberExpression memberExpression)
        {
            return memberExpression.Member.Name;
        }
        throw new ArgumentException("Invalid property expression");
    }
}