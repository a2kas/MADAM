using System.Reflection;
using NUnit.Framework;
using Shouldly;
using Tamro.Madam.Models.Common;

namespace Tamro.Madam.Models.Tests.Common;

[TestFixture]
public class BaseDataGridModelTests
{
    [Test]
    public void AllDataGridModels_MustHave_AValidKey()
    {
        // Arrange
        var assembly = typeof(BaseDataGridModel<>).Assembly;

        var baseGenericType = typeof(BaseDataGridModel<>);
        var invalidModels = new List<string>();


        var modelTypes = assembly.GetTypes().Where(t =>
            t.IsClass &&
            !t.IsAbstract &&
            t.BaseType != null &&
            t.BaseType.IsGenericType &&
            t.BaseType.GetGenericTypeDefinition() == baseGenericType);

        // Act
        foreach (var modelType in modelTypes)
        {
            var hasAttributeKey = modelType
                .GetProperties()
                .Any(p => p.IsDefined(typeof(DataGridIdentifierAttribute), inherit: true));

            var hasIdKey = modelType
                .GetProperty("Id", BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase) != null;

            if (!hasAttributeKey && !hasIdKey)
            {
                invalidModels.Add(modelType.Name);
            }
        }

        // Assert
        invalidModels.ShouldBeEmpty();
    }
}
