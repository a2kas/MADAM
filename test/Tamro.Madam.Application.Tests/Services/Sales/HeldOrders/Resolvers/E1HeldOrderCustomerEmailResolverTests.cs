using AutoFixture;
using Moq;
using NUnit.Framework;
using Shouldly;
using Tamro.Madam.Application.Services.Customers.Factories;
using Tamro.Madam.Application.Services.Sales.HeldOrders.Resolvers;
using Tamro.Madam.Models.Customers.Wholesale;
using Tamro.Madam.Models.General;
using Tamro.Madam.Models.Sales.HeldOrders;
using Tamro.Madam.Repository.Repositories.Customers.Wholesale;

namespace Tamro.Madam.Application.Tests.Services.Sales.HeldOrders.Resolvers;

[TestFixture]
public class E1HeldOrderCustomerEmailResolverTests
{
    private Fixture _fixture;

    private Mock<IWholesaleCustomerRepository> _wholesaleCustomerRepository;
    private Mock<IWholesaleCustomerRepositoryFactory> _wholesaleCustomerRepositoryFactory;

    private E1HeldOrderCustomerEmailResolver _e1HeldOrderCustomerEmailResolver;

    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture();

        _wholesaleCustomerRepositoryFactory = new Mock<IWholesaleCustomerRepositoryFactory>();
        _wholesaleCustomerRepository = new Mock<IWholesaleCustomerRepository>();
        _wholesaleCustomerRepositoryFactory.Setup(x => x.Get(It.IsAny<BalticCountry>())).Returns(_wholesaleCustomerRepository.Object);

        _e1HeldOrderCustomerEmailResolver = new E1HeldOrderCustomerEmailResolver(_wholesaleCustomerRepositoryFactory.Object);
    }

    [Test]
    public async Task Resolve_GetFactory()
    {
        // Arrange
        var country = BalticCountry.LV;
        var orders = _fixture.CreateMany<E1HeldOrderModel>().ToList();
        _wholesaleCustomerRepository.Setup(x => x.GetMany(It.IsAny<WholesaleCustomerSearchModel>())).ReturnsAsync([]);

        // Act
        await _e1HeldOrderCustomerEmailResolver.Resolve(orders, country);

        // Assert
        _wholesaleCustomerRepositoryFactory.Verify(x => x.Get(country), Times.Once);
    }

    [Test]
    public async Task Resolve_ResolvesOrderCorrectly()
    {
        // Arrange
        var country = BalticCountry.LV;
        var orders = _fixture.CreateMany<E1HeldOrderModel>(2).ToList();
        var notificationRecipients = _fixture.CreateMany<WholesaleCustomerModel>(3).ToList();
        notificationRecipients[1].AddressNumber = orders[1].E1ShipTo;

        _wholesaleCustomerRepository.Setup(x => x.GetMany(It.IsAny<WholesaleCustomerSearchModel>())).ReturnsAsync(notificationRecipients);

        // Act
        await _e1HeldOrderCustomerEmailResolver.Resolve(orders, country);

        // Assert
        orders[1].Email.ShouldBe(notificationRecipients[1].EmailAddress);
        orders[1].ResponsibleEmployeeNumber.ShouldBe(notificationRecipients[1].ResponsibleEmployeeNumber);
        orders[1].MailingName.ShouldBe(notificationRecipients[1].MailingName);
    }
}
