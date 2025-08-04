using Shouldly;
using NUnit.Framework;
using System.Reflection;
using Tamro.Madam.Models.Data;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items;
using TamroUtilities.EFCore.Models;
using Z.EntityFramework.Plus;

namespace Tamro.Madam.Models.Tests.Data;

[TestFixture]
public class AuditDataTests
{ 
    [Test]
    public void AllAuditableEntities_ShouldBeFoundInDictionary()
    {
        // Arrange
        var assembly = Assembly.GetAssembly(typeof(Item));
        var auditableTypes = assembly.GetTypes().Where(t => typeof(IAuditable).IsAssignableFrom(t) && t.IsClass);

        // Act + Assert
        foreach (var auditableType in auditableTypes)
        {
            AuditData.AuditEntryDisplayNames.TryGetValue(auditableType.Name, out var displayName).ShouldBeTrue();
        }
    }

    [Test]
    public void AllAuditStates_ShouldBeFoundInDictionary()
    {
        // Arrange
        var auditStates = Enum.GetNames(typeof(AuditEntryState));

        // Act + Assert
        foreach (var auditState in auditStates)
        {
            AuditData.AuditStateDisplayNames.TryGetValue(auditState, out var displayName).ShouldBeTrue($"{displayName} does not have audit name");
        }
    }
}