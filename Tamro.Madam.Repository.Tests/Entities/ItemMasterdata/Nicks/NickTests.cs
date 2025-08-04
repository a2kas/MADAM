using Shouldly;
using NUnit.Framework;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Nicks;
using TamroUtilities.EFCore.Models;

namespace Tamro.Madam.Repository.Tests.Entities.ItemMasterdata.Nicks;

[TestFixture]
public class NickTests
{
    [Test]
    public void Nick_Implements_IAuditable()
    {
        // Act + Assert
        typeof(IAuditable).IsAssignableFrom(typeof(Nick)).ShouldBeTrue();
    }
}