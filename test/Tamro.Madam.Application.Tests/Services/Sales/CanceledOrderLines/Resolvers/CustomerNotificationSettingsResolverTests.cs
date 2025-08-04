using AutoFixture;
using Shouldly;
using Moq;
using NUnit.Framework;
using System.Linq.Expressions;
using Tamro.Madam.Application.Services.Sales.CanceledOrderLines.Resolvers;
using Tamro.Madam.Models.General;
using Tamro.Madam.Models.Sales.CanceledOrderLines;
using Tamro.Madam.Repository.Common;
using Tamro.Madam.Repository.Entities.Customers;
using Tamro.Madam.Repository.Repositories.Customers;

namespace Tamro.Madam.Application.Tests.Services.Sales.CanceledOrderLines.Resolvers;

[TestFixture]
public class CustomerNotificationSettingsResolverTests
{
    private Fixture _fixture;

    private Mock<ICustomerLegalEntityRepository> _customerLegalEntityRepository;

    private CustomerNotificationSettingsResolver _customerLegalNotificationSettingsResolver;

    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture();

        _customerLegalEntityRepository = new Mock<ICustomerLegalEntityRepository>();

        _customerLegalNotificationSettingsResolver = new CustomerNotificationSettingsResolver(_customerLegalEntityRepository.Object);
    }

    [Test]
    public void CustomerEmailResolver_PriorityShouldBeEqualTo2()
    {
        // Assert
        _customerLegalNotificationSettingsResolver.Priority.ShouldBe(2);
    }

    [Test]
    public async Task Resolve_GetsNotificationSettingsBySoldTos()
    {
        // Arrange
        var country = BalticCountry.LV;
        var orders = _fixture.CreateMany<CanceledOrderHeaderModel>().ToList();
        orders[0].SoldTo = 1;
        orders[1].SoldTo = 2;
        orders[2].SoldTo = 1;
        _customerLegalEntityRepository.Setup(x => x.GetMany(It.IsAny<Expression<Func<CustomerLegalEntity, bool>>>(),
            It.Is<List<IncludeOperation<CustomerLegalEntity>>>(y => y.Count == 1), false, It.IsAny<CancellationToken>())).ReturnsAsync([]);

        // Act
        await _customerLegalNotificationSettingsResolver.Resolve(orders, country);

        // Assert
        _customerLegalEntityRepository.Verify(x => x.GetMany(It.Is<Expression<Func<CustomerLegalEntity, bool>>>(y => y.ToString().Contains(nameof(CustomerLegalEntity.E1SoldTo))),
            It.Is<List<IncludeOperation<CustomerLegalEntity>>>(y => y.Count == 1), false, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task Resolve_ResolvesSendCanceledOrderNotification()
    {
        // Arrange
        var country = BalticCountry.LV;
        var orders = _fixture.CreateMany<CanceledOrderHeaderModel>(2).ToList();
        orders[0].SoldTo = 1;
        orders[1].SoldTo = 2;
        var customerLegalEntities = _fixture.CreateMany<CustomerLegalEntity>().ToList();
        customerLegalEntities[0].E1SoldTo = 2;
        customerLegalEntities[1].E1SoldTo = 1;
        _customerLegalEntityRepository.Setup(x => x.GetMany(It.IsAny<Expression<Func<CustomerLegalEntity, bool>>>(),
                    It.Is<List<IncludeOperation<CustomerLegalEntity>>>(y => y.Count == 1), false, It.IsAny<CancellationToken>())).ReturnsAsync(customerLegalEntities);

        // Act
        await _customerLegalNotificationSettingsResolver.Resolve(orders, country);

        // Assert
        orders[0].SendCanceledOrderNotification.ShouldBe(customerLegalEntities[1].NotificationSettings.SendCanceledOrderNotification);
        orders[1].SendCanceledOrderNotification.ShouldBe(customerLegalEntities[0].NotificationSettings.SendCanceledOrderNotification);
    }
}
