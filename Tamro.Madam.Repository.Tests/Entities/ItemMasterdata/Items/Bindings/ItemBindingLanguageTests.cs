using Shouldly;
using NUnit.Framework;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items.Bindings;
using TamroUtilities.EFCore.Models;

namespace Tamro.Madam.Repository.Tests.Entities.ItemMasterdata.Items.Bindings.ItemBindingLanguages;

[TestFixture]
public class ItemBindingLanguageTests
{
    [Test]
    public void ItemBindingLanguage_Implements_IAuditable()
    {
        // Act + Assert
        typeof(IAuditable).IsAssignableFrom(typeof(ItemBindingLanguage)).ShouldBeTrue();
    }
}