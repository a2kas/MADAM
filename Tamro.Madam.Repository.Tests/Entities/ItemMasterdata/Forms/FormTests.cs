using Shouldly;
using NUnit.Framework;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Forms;
using TamroUtilities.EFCore.Models;

namespace Tamro.Madam.Repository.Tests.Entities.ItemMasterdata.Forms;

[TestFixture]
public class FormTests
{
    [Test]
    public void Form_Implements_IAuditable()
    {
        // Act + Assert
        typeof(IAuditable).IsAssignableFrom(typeof(Form)).ShouldBeTrue();
    }
}