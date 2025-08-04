using Shouldly;
using NUnit.Framework;
using Tamro.Madam.Models.Data;
using Tamro.Madam.Models.Overview;

namespace Tamro.Madam.Models.Tests.Overview;

[TestFixture]
public class AuditEntriesByEntityCountModelTests
{
    [Test]
    public void DisplayName_WhenKeyExists_ReturnsDisplayName()
    {
        // Arrange
        const string entityName = "Atc";
        var yourClassInstance = new AuditEntriesByEntityCountModel()
        {
            EntityName = entityName
        };

        // Act
        var result = yourClassInstance.DisplayName;

        // Assert
        result.ShouldBe(AuditData.AuditEntryDisplayNames[entityName]);
    }

    [Test]
    public void DisplayName_WhenKeyDoesNotExist_ReturnsEntityName()
    {
        // Arrange
        const string entityName = "abcdefghijklmidontexist";
        var yourClassInstance = new AuditEntriesByEntityCountModel()
        {
            EntityName = entityName
        };

        // Act
        var result = yourClassInstance.DisplayName;

        // Assert
        result.ShouldBe(entityName);
    }
}