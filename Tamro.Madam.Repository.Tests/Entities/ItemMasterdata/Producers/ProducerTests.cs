using Shouldly;
using NUnit.Framework;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Producers;
using TamroUtilities.EFCore.Models;

namespace Tamro.Madam.Repository.Tests.Entities.Producers;

[TestFixture]
public class ProducerTests
{
    [Test]
    public void Producer_Implements_IAuditable()
    {
        // Act + Assert
        typeof(IAuditable).IsAssignableFrom(typeof(Producer)).ShouldBeTrue();
    }
}