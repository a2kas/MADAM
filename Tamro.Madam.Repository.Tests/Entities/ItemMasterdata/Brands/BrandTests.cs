using Shouldly;
using NUnit.Framework;
using Tamro.Madam.Repository.Entities.Brands;
using TamroUtilities.EFCore.Models;

namespace Tamro.Madam.Repository.Tests.Entities.ItemMasterdata.Brands;

[TestFixture]
public class BrandTests
{
    [Test]
    public void Brand_Implements_IAuditable()
    {
        // Act + Assert
        typeof(IAuditable).IsAssignableFrom(typeof(Brand)).ShouldBeTrue();
    }
}