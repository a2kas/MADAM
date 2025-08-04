using Shouldly;
using NUnit.Framework;
using Tamro.Madam.Application.Commands.Commerce.ItemAssortmentSalesChannels;
using Tamro.Madam.Models.Commerce.ItemAssortmentSalesChannels;

namespace Tamro.Madam.Application.Tests.Commands.Commerce.ItemAssortmentSalesChannels;

[TestFixture]
public class UpsertItemAssortmentSalesChannelCommandTests
{
    [Test]
    public void Ctor_Sets_Model()
    {
        // Act
        var cmd = new UpsertItemAssortmentSalesChannelCommand(new ItemAssortmentSalesChannelDetailsModel() { Name = "Test", });

        // Assert
        cmd.Model.Name.ShouldBe("Test");
    }
}