using Shouldly;
using NUnit.Framework;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items.Bindings;
using TamroUtilities.EFCore.Models;

namespace Tamro.Madam.Repository.Tests.Entities.ItemMasterdata.Items.Bindings.ItemBindings;

[TestFixture]
public class ItemBindingTests
{
    [Test]
    public void ItemBinding_Implements_IAuditable()
    {
        // Act + Assert
        typeof(IAuditable).IsAssignableFrom(typeof(ItemBinding)).ShouldBeTrue();
    }
}