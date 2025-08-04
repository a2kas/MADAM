using Shouldly;
using Moq;
using NUnit.Framework;
using Tamro.Madam.Application.Commands.Commerce.ItemAssortmentSalesChannels;
using Tamro.Madam.Application.Handlers.Commerce.ItemAssortmentSalesChannels;
using Tamro.Madam.Models.Commerce.ItemAssortmentSalesChannels;
using Tamro.Madam.Models.Commerce.ItemAssortmentSalesChannels.Import;
using Tamro.Madam.Models.General;
using Tamro.Madam.Repository.Repositories.ItemMasterdata.Items.Bindings;

namespace Tamro.Madam.Application.Tests.Handlers.Commerce.ItemAssortmentSalesChannels;

[TestFixture]
public class GetImportedAssortmentCommandHandlerTests
{
    private MockRepository _mockRepository;

    private Mock<IItemBindingRepository> _itemBindingRepository;

    private GetImportedAssortmentCommandHandler _getImportedAssortmentCommandHandler;

    [SetUp]
    public void SetUp()
    {
        _mockRepository = new MockRepository(MockBehavior.Loose);

        _itemBindingRepository = _mockRepository.Create<IItemBindingRepository>();

        _getImportedAssortmentCommandHandler = new GetImportedAssortmentCommandHandler(_itemBindingRepository.Object);
    }

    [Test]
    public async Task Handle_ItemAssortmentImport_IsReturnedCorrectly()
    {
        // Arrange
        var model = new ItemAssortmentImportModel()
        {
            ItemNos = "1004 5006 9004 1004",
            Country = BalticCountry.LV,
        };
        var cmd = new GetImportedAssortmentCommand(model);
        _itemBindingRepository.Setup(x => x.GetSalesChannelItemAssortmentOverview(It.Is<IEnumerable<string>>(y => y.Count() == 3), It.IsAny<IEnumerable<string>>(), CancellationToken.None))
            .ReturnsAsync(new List<ItemAssortmentItemModel>()
            {
                new()
                {
                    ItemNo = "1004",
                },
                new()
                {
                    ItemNo = "5006",
                },
            });

        // Act
        var result = await _getImportedAssortmentCommandHandler.Handle(cmd, CancellationToken.None);

        // Assert
        result.Data.Count().ShouldBe(3);
    }
}