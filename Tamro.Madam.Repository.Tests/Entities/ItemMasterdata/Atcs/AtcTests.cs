using Shouldly;
using NUnit.Framework;
using Tamro.Madam.Repository.Entities.Atcs;
using TamroUtilities.EFCore.Models;

namespace Tamro.Madam.Repository.Tests.Entities.ItemMasterdata.Atcs;

[TestFixture]
public class AtcTests
{
    [Test]
    public void Atc_Implements_IAuditable()
    {
        // Act + Assert
        typeof(IAuditable).IsAssignableFrom(typeof(Atc)).ShouldBeTrue();
    }
}