using AutoFixture;
using Moq;
using NUnit.Framework;
using Shouldly;
using Tamro.Madam.Application.Services.Customers.Factories;
using Tamro.Madam.Application.Services.Sales.HeldOrders.Resolvers;
using Tamro.Madam.Models.Employees.Wholesale;
using Tamro.Madam.Models.General;
using Tamro.Madam.Models.Sales.HeldOrders;
using Tamro.Madam.Repository.Repositories.Customers.Wholesale;

namespace Tamro.Madam.Application.Tests.Services.Sales.HeldOrders.Resolvers;

[TestFixture]
public class E1HeldOrderEmployeeEmailResolverTests
{
    private Fixture _fixture;

    private Mock<IWholesaleEmployeeRepository> _wholesaleEmployeeRepository;
    private Mock<IWholesaleEmployeeRepositoryFactory> _wholesaleEmployeeRepositoryFactory;

    private E1HeldOrderEmployeeEmailResolver _e1HeldOrderEmployeeEmailResolver;

    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture();

        _wholesaleEmployeeRepository = new Mock<IWholesaleEmployeeRepository>();
        _wholesaleEmployeeRepositoryFactory = new Mock<IWholesaleEmployeeRepositoryFactory>();
        _wholesaleEmployeeRepositoryFactory.Setup(x => x.Get(It.IsAny<BalticCountry>())).Returns(_wholesaleEmployeeRepository.Object);

        _e1HeldOrderEmployeeEmailResolver = new E1HeldOrderEmployeeEmailResolver(_wholesaleEmployeeRepositoryFactory.Object);
    }

    [Test]
    public async Task Resolve_GetFactory()
    {
        // Arrange
        var country = BalticCountry.LV;
        var orders = _fixture.CreateMany<E1HeldOrderModel>().ToList();
        _wholesaleEmployeeRepository.Setup(x => x.GetMany(It.IsAny<WholesaleEmployeeSearchModel>())).ReturnsAsync([]);

        // Act
        await _e1HeldOrderEmployeeEmailResolver.Resolve(orders, country);

        // Assert
        _wholesaleEmployeeRepositoryFactory.Verify(x => x.Get(country), Times.Once);
    }

    [Test]
    public async Task Resolve_ResolvesOrderCorrectly()
    {
        // Arrange
        var country = BalticCountry.LV;
        var orders = _fixture.CreateMany<E1HeldOrderModel>(2).ToList();
        var notificationRecipients = _fixture.CreateMany<WholesaleEmployeeModel>(3).ToList();
        notificationRecipients[1].AddressNumber = orders[1].ResponsibleEmployeeNumber;

        _wholesaleEmployeeRepository.Setup(x => x.GetMany(It.IsAny<WholesaleEmployeeSearchModel>())).ReturnsAsync(notificationRecipients);

        // Act
        await _e1HeldOrderEmployeeEmailResolver.Resolve(orders, country);

        // Assert
        orders[1].EmployeesEmail.ShouldBe(notificationRecipients[1].Email);
    }
}
