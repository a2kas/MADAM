using AutoFixture;
using Shouldly;
using Moq;
using NUnit.Framework;
using Tamro.Madam.Application.Services.Items.Wholesale.Factories;
using Tamro.Madam.Application.Services.Sales.CanceledOrderLines.Resolvers;
using Tamro.Madam.Models.General;
using Tamro.Madam.Models.ItemMasterdata.Items.Wholesale;
using Tamro.Madam.Models.Sales.CanceledOrderLines;
using Tamro.Madam.Repository.Repositories.ItemMasterdata.Items.Wholesale;

namespace Tamro.Madam.Application.Tests.Services.Sales.CanceledOrderLines.Resolvers;

[TestFixture]
public class ItemDescriptionResolverTests
{
    private Fixture _fixture;

    private Mock<IWholesaleItemRepository> _wholesaleItemRepository;

    private Mock<IWholesaleItemRepositoryFactory> _wholesaleItemRepositoryFactory;

    private ItemDescriptionResolver _itemDescriptionResolver;

    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture();

        _wholesaleItemRepository = new Mock<IWholesaleItemRepository>();

        _wholesaleItemRepositoryFactory = new Mock<IWholesaleItemRepositoryFactory>();
        _wholesaleItemRepositoryFactory.Setup(x => x.Get(It.IsAny<BalticCountry>())).Returns(_wholesaleItemRepository.Object);

        _itemDescriptionResolver = new ItemDescriptionResolver(_wholesaleItemRepositoryFactory.Object);
    }

    [Test]
    public async Task ItemDescriptionResolver_PriorityShouldBeEqualTo3()
    {
        // Assert
        _itemDescriptionResolver.Priority.ShouldBe(3);
    }

    [Test]
    public async Task Resolve_GetsItemsByFactory()
    {
        // Arrange
        var country = BalticCountry.LV;
        var orders = _fixture.CreateMany<CanceledOrderHeaderModel>().ToList();
        _wholesaleItemRepository.Setup(x => x.GetMany(It.IsAny<WholesaleItemSearchModel>())).ReturnsAsync([]);

        // Act
        await _itemDescriptionResolver.Resolve(orders, country);

        // Assert
        _wholesaleItemRepositoryFactory.Verify(x => x.Get(country), Times.Once);
    }

    [Test]
    public async Task Resolve_GetsItemsByDistinctItemNo()
    {
        // Arrange
        var country = BalticCountry.LV;
        var orders = _fixture.CreateMany<CanceledOrderHeaderModel>().ToList();
        orders[0].SendCanceledOrderNotification = true;
        orders[0].Lines.ToList()[0].EmailStatus = CanceledOrderLineEmailStatus.NotSent;
        orders[0].Lines.ToList()[1].EmailStatus = CanceledOrderLineEmailStatus.NotSent;
        orders[0].Lines.ToList()[2].EmailStatus = CanceledOrderLineEmailStatus.NotSent;
        orders[0].Lines.ToList()[0].ItemNo = "item1";
        orders[0].Lines.ToList()[1].ItemNo = "item2";
        orders[0].Lines.ToList()[2].ItemNo = "item3";
        orders[1].SendCanceledOrderNotification = true;
        orders[1].Lines.ToList()[0].EmailStatus = CanceledOrderLineEmailStatus.NotSent;
        orders[1].Lines.ToList()[1].EmailStatus = CanceledOrderLineEmailStatus.NotSent;
        orders[1].Lines.ToList()[2].EmailStatus = CanceledOrderLineEmailStatus.NotSent;
        orders[1].Lines.ToList()[0].ItemNo = "item4";
        orders[1].Lines.ToList()[1].ItemNo = "item5";
        orders[1].Lines.ToList()[2].ItemNo = "item6";
        orders[2].SendCanceledOrderNotification = true;
        orders[2].Lines.ToList()[0].EmailStatus = CanceledOrderLineEmailStatus.NotSent;
        orders[2].Lines.ToList()[1].EmailStatus = CanceledOrderLineEmailStatus.NotSent;
        orders[2].Lines.ToList()[2].EmailStatus = CanceledOrderLineEmailStatus.NotSent;
        orders[2].Lines.ToList()[0].ItemNo = "item1";
        orders[2].Lines.ToList()[1].ItemNo = "item2";
        orders[2].Lines.ToList()[2].ItemNo = "item3";
        _wholesaleItemRepository.Setup(x => x.GetMany(It.IsAny<WholesaleItemSearchModel>())).ReturnsAsync([]);

        // Act
        await _itemDescriptionResolver.Resolve(orders, country);

        // Assert
        _wholesaleItemRepository.Verify(x => x.GetMany(It.Is<WholesaleItemSearchModel>(
            y => y.ItemNo2s != null &&
                 y.ItemNo2s.Count() == 6 &&
                 y.ItemNo2s.Contains("item1") &&
                 y.ItemNo2s.Contains("item2") &&
                 y.ItemNo2s.Contains("item3") &&
                 y.ItemNo2s.Contains("item4") &&
                 y.ItemNo2s.Contains("item5") &&
                 y.ItemNo2s.Contains("item6")))
        , Times.Once);
    }

    [Test]
    public async Task Resolve_ResolvesItemDescription()
    {
        // Arrange
        var country = BalticCountry.LV;
        var orders = _fixture.CreateMany<CanceledOrderHeaderModel>(2).ToList();
        orders[0].SendCanceledOrderNotification = true;
        orders[0].Lines.ToList()[0].EmailStatus = CanceledOrderLineEmailStatus.NotSent;
        orders[0].Lines.ToList()[1].EmailStatus = CanceledOrderLineEmailStatus.NotSent;
        orders[0].Lines.ToList()[2].EmailStatus = CanceledOrderLineEmailStatus.NotSent;
        orders[0].Lines.ToList()[0].ItemNo = "item1";
        orders[0].Lines.ToList()[1].ItemNo = "item2";
        orders[0].Lines.ToList()[1].ItemName = "NotChanged";
        orders[0].Lines.ToList()[2].ItemNo = "item3";
        orders[1].SendCanceledOrderNotification = true;
        orders[1].Lines.ToList()[0].EmailStatus = CanceledOrderLineEmailStatus.NotSent;
        orders[1].Lines.ToList()[1].EmailStatus = CanceledOrderLineEmailStatus.NotSent;
        orders[1].Lines.ToList()[2].EmailStatus = CanceledOrderLineEmailStatus.NotSent;
        orders[1].Lines.ToList()[0].ItemNo = "item4";
        orders[1].Lines.ToList()[1].ItemNo = "item5";
        orders[1].Lines.ToList()[2].ItemNo = "item6";

        var items = _fixture.CreateMany<WholesaleItemModel>().ToList();
        items[0].ItemNo = "item1";
        items[1].ItemNo = "wrong item";
        items[2].ItemNo = "item5";
        _wholesaleItemRepository.Setup(x => x.GetMany(It.IsAny<WholesaleItemSearchModel>())).ReturnsAsync(items);

        // Act
        await _itemDescriptionResolver.Resolve(orders, country);

        // Assert
        orders[0].Lines.ToList()[0].ItemName.ShouldBe(items[0].ItemDescription);
        orders[0].Lines.ToList()[1].ItemName.ShouldBe("NotChanged");
        orders[1].Lines.ToList()[1].ItemName.ShouldBe(items[2].ItemDescription);
    }
}
