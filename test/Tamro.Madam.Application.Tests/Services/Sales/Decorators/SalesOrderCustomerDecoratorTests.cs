using Moq;
using NUnit.Framework;
using Shouldly;
using Tamro.Madam.Application.Services.Customers.Factories;
using Tamro.Madam.Application.Services.Sales.Decorators;
using Tamro.Madam.Models.Customers.Wholesale;
using Tamro.Madam.Models.Customers.Wholesale.Clsf;
using Tamro.Madam.Models.General;
using Tamro.Madam.Models.Sales;
using Tamro.Madam.Models.Sales.CanceledOrderLines;
using Tamro.Madam.Repository.Common;
using Tamro.Madam.Repository.Repositories.Customers.Wholesale;

namespace Tamro.Madam.Application.Tests.Services.Sales.Decorators;

[TestFixture]
public class SalesOrderCustomerDecoratorTests
{
    private MockRepository _mockRepository;

    private Mock<IWholesaleCustomerRepositoryFactory> _wholesaleCustomerRepositoryFactory;

    private SalesOrderCustomerDecorator _salesOrderCustomerDecorator;

    [SetUp]
    public void SetUp()
    {
        _mockRepository = new MockRepository(MockBehavior.Loose);

        _wholesaleCustomerRepositoryFactory = _mockRepository.Create<IWholesaleCustomerRepositoryFactory>();

        _salesOrderCustomerDecorator = new SalesOrderCustomerDecorator(_wholesaleCustomerRepositoryFactory.Object);
    }

    [Test]
    public async Task Decorate_WhenCalled_ShouldSetCustomerNamesOnOrders()
    {
        // Arrange
        var orders = new List<ISalesOrderHeader>
        {
            new CanceledOrderHeaderModel { E1ShipTo = 123 },
            new CanceledOrderHeaderModel { E1ShipTo = 456 },
            new CanceledOrderHeaderModel { E1ShipTo = 789 }
        };

        var country = BalticCountry.LT;

        var customers = new List<WholesaleCustomerClsfModel>
        {
            new WholesaleCustomerClsfModel { AddressNumber = 123, Name = "Customer A" },
            new WholesaleCustomerClsfModel { AddressNumber = 456, Name = "Customer B" }
        };

        var wholesaleCustomerRepository = new Mock<IWholesaleCustomerRepository>();
        _wholesaleCustomerRepositoryFactory.Setup(x => x.Get(BalticCountry.LT)).Returns(wholesaleCustomerRepository.Object);
        wholesaleCustomerRepository
            .Setup(x => x.GetClsf(
                It.Is<IEnumerable<int>>(y => y.Count() == 3),
                WholesaleCustomerType.All,
                1,
                int.MaxValue))
            .ReturnsAsync(new PaginatedData<WholesaleCustomerClsfModel>(customers, 2, 1, 1));

        // Act
        await _salesOrderCustomerDecorator.Decorate(orders, country);

        // Assert
        orders[0].CustomerName.ShouldBe("Customer A");
        orders[1].CustomerName.ShouldBe("Customer B");
        orders[2].CustomerName.ShouldBeNull();
    }
}