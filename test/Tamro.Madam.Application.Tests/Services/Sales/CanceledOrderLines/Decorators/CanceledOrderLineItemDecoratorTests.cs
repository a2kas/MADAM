using AutoFixture;
using Shouldly;
using Moq;
using NUnit.Framework;
using Tamro.Madam.Application.Services.Items.Wholesale.Factories;
using Tamro.Madam.Application.Services.Sales.CanceledOrderLines.Decorators;
using Tamro.Madam.Models.General;
using Tamro.Madam.Models.ItemMasterdata.Items.Wholesale.Clsf;
using Tamro.Madam.Models.Sales.CanceledOrderLines.Statistics;
using Tamro.Madam.Repository.Common;
using Tamro.Madam.Repository.Repositories.ItemMasterdata.Items.Wholesale;

namespace Tamro.Madam.Application.Tests.Services.Sales.CanceledOrderLines.Decorators;
[TestFixture]
public class CanceledOrderLineItemDecoratorTests
{
    private Fixture _fixture;

    private Mock<IWholesaleItemRepositoryFactory> _wholesaleItemRepositoryFactory;
    private Mock<IWholesaleItemRepository> _wholesaleItemRepository;

    private CanceledOrderLineItemDecorator _canceledOrderLineItemDecorator;

    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture();

        _wholesaleItemRepositoryFactory = new Mock<IWholesaleItemRepositoryFactory>();
        _wholesaleItemRepository = new Mock<IWholesaleItemRepository>();

        _canceledOrderLineItemDecorator = new CanceledOrderLineItemDecorator(_wholesaleItemRepositoryFactory.Object);
    }

    [Test]
    public async Task Decorate_ItemsExist_DecoratesLinesWithDescriptionsCorrectly()
    {
        // Arrange
        var country = BalticCountry.LV;
        var lines = _fixture.Build<CanceledLineStatisticModel>()
            .With(x => x.ItemName, default(string))
            .CreateMany(3)
            .ToList();

        var items = new List<WholesaleItemClsfModel>
        {
            new() {
                ItemNo = lines[0].ItemNo,
                Name = "Item 1 name",
            },
                new WholesaleItemClsfModel
            {
                ItemNo = lines[2].ItemNo,
                Name = "Item 3 name",
            }
        };

        _wholesaleItemRepositoryFactory.Setup(x => x.Get(country)).Returns(_wholesaleItemRepository.Object);
        _wholesaleItemRepository.Setup(x => x.GetClsf(It.IsAny<List<string>>(), 1, int.MaxValue)).ReturnsAsync(new PaginatedData<WholesaleItemClsfModel>(items, items.Count(), 1, int.MaxValue));

        // Act
        await _canceledOrderLineItemDecorator.Decorate(lines, country);

        // Assert
        lines[0].ItemName.ShouldBe("Item 1 name");
        lines[1].ItemName.ShouldBeNull();
        lines[2].ItemName.ShouldBe("Item 3 name");
    }
}
