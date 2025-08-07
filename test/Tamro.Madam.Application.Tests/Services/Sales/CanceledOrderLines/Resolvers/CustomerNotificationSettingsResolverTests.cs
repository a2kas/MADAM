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
    private Mock<ICustomerRepository> _customerRepository;
    private CustomerNotificationSettingsResolver _customerNotificationSettingsResolver;

    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture();
        _customerRepository = new Mock<ICustomerRepository>();
        _customerNotificationSettingsResolver = new CustomerNotificationSettingsResolver(_customerRepository.Object);
    }

    [Test]
    public void CustomerNotificationSettingsResolver_PriorityShouldBeEqualTo2()
    {
        // Assert
        _customerNotificationSettingsResolver.Priority.ShouldBe(2);
    }

    [Test]
    public async Task Resolve_GetsNotificationSettingsByE1ShipTos()
    {
        // Arrange
        var country = BalticCountry.LV;
        var orders = _fixture.CreateMany<CanceledOrderHeaderModel>().ToList();
        orders[0].E1ShipTo = 1001;
        orders[1].E1ShipTo = 1002;
        orders[2].E1ShipTo = 1001;

        _customerRepository.Setup(x => x.GetMany(
            It.IsAny<Expression<Func<Customer, bool>>>(),
            It.Is<List<IncludeOperation<Customer>>>(y => y.Count == 2),
            false,
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Customer>());

        // Act
        await _customerNotificationSettingsResolver.Resolve(orders, country);

        // Assert
        _customerRepository.Verify(x => x.GetMany(
            It.Is<Expression<Func<Customer, bool>>>(y => y.ToString().Contains(nameof(Customer.E1ShipTo))),
            It.Is<List<IncludeOperation<Customer>>>(y => y.Count == 2),
            false,
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task Resolve_ResolvesSendCanceledOrderNotification_FromCustomerNotification()
    {
        // Arrange
        var country = BalticCountry.LV;
        var orders = _fixture.CreateMany<CanceledOrderHeaderModel>(2).ToList();
        orders[0].E1ShipTo = 1001;
        orders[1].E1ShipTo = 1002;

        var customers = new List<Customer>
        {
            new Customer
            {
                E1ShipTo = 1001,
                CustomerNotification = new CustomerNotification { SendCanceledOrderNotification = false }
            },
            new Customer
            {
                E1ShipTo = 1002,
                CustomerNotification = new CustomerNotification { SendCanceledOrderNotification = true }
            }
        };

        _customerRepository.Setup(x => x.GetMany(
            It.IsAny<Expression<Func<Customer, bool>>>(),
            It.Is<List<IncludeOperation<Customer>>>(y => y.Count == 2),
            false,
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(customers);

        // Act
        await _customerNotificationSettingsResolver.Resolve(orders, country);

        // Assert
        orders[0].SendCanceledOrderNotification.ShouldBe(false);
        orders[1].SendCanceledOrderNotification.ShouldBe(true);
    }

    [Test]
    public async Task Resolve_ResolvesSendCanceledOrderNotification_FromCustomerLegalEntitySettings()
    {
        // Arrange
        var country = BalticCountry.LV;
        var orders = _fixture.CreateMany<CanceledOrderHeaderModel>(2).ToList();
        orders[0].E1ShipTo = 1001;
        orders[1].E1ShipTo = 1002;

        var customers = new List<Customer>
        {
            new Customer
            {
                E1ShipTo = 1001,
                CustomerNotification = null, // No customer-specific setting
                CustomerLegalEntity = new CustomerLegalEntity
                {
                    NotificationSettings = new CustomerLegalEntityNotification
                    {
                        SendCanceledOrderNotification = false
                    }
                }
            },
            new Customer
            {
                E1ShipTo = 1002,
                CustomerNotification = null, // No customer-specific setting
                CustomerLegalEntity = new CustomerLegalEntity
                {
                    NotificationSettings = new CustomerLegalEntityNotification
                    {
                        SendCanceledOrderNotification = true
                    }
                }
            }
        };

        _customerRepository.Setup(x => x.GetMany(
            It.IsAny<Expression<Func<Customer, bool>>>(),
            It.Is<List<IncludeOperation<Customer>>>(y => y.Count == 2),
            false,
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(customers);

        // Act
        await _customerNotificationSettingsResolver.Resolve(orders, country);

        // Assert
        orders[0].SendCanceledOrderNotification.ShouldBe(false);
        orders[1].SendCanceledOrderNotification.ShouldBe(true);
    }

    [Test]
    public async Task Resolve_CustomerNotificationTakesPriorityOverLegalEntitySetting()
    {
        // Arrange
        var country = BalticCountry.LV;
        var orders = _fixture.CreateMany<CanceledOrderHeaderModel>(1).ToList();
        orders[0].E1ShipTo = 1001;

        var customers = new List<Customer>
        {
            new Customer
            {
                E1ShipTo = 1001,
                CustomerNotification = new CustomerNotification { SendCanceledOrderNotification = false }, // Customer setting
                CustomerLegalEntity = new CustomerLegalEntity
                {
                    NotificationSettings = new CustomerLegalEntityNotification
                    {
                        SendCanceledOrderNotification = true  // Legal entity setting (should be ignored)
                    }
                }
            }
        };

        _customerRepository.Setup(x => x.GetMany(
            It.IsAny<Expression<Func<Customer, bool>>>(),
            It.Is<List<IncludeOperation<Customer>>>(y => y.Count == 2),
            false,
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(customers);

        // Act
        await _customerNotificationSettingsResolver.Resolve(orders, country);

        // Assert
        orders[0].SendCanceledOrderNotification.ShouldBe(false); // Should use customer setting, not legal entity
    }

    [Test]
    public async Task Resolve_DefaultsToTrueWhenNoSettingsFound()
    {
        // Arrange
        var country = BalticCountry.LV;
        var orders = _fixture.CreateMany<CanceledOrderHeaderModel>(1).ToList();
        orders[0].E1ShipTo = 1001;

        var customers = new List<Customer>
        {
            new Customer
            {
                E1ShipTo = 1001,
                CustomerNotification = null,
                CustomerLegalEntity = new CustomerLegalEntity
                {
                    NotificationSettings = null // No settings at all
                }
            }
        };

        _customerRepository.Setup(x => x.GetMany(
            It.IsAny<Expression<Func<Customer, bool>>>(),
            It.Is<List<IncludeOperation<Customer>>>(y => y.Count == 2),
            false,
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(customers);

        // Act
        await _customerNotificationSettingsResolver.Resolve(orders, country);

        // Assert
        orders[0].SendCanceledOrderNotification.ShouldBe(true); // Should default to true
    }

    [Test]
    public async Task Resolve_DefaultsToTrueWhenCustomerNotFound()
    {
        // Arrange
        var country = BalticCountry.LV;
        var orders = _fixture.CreateMany<CanceledOrderHeaderModel>(1).ToList();
        orders[0].E1ShipTo = 1001;

        _customerRepository.Setup(x => x.GetMany(
            It.IsAny<Expression<Func<Customer, bool>>>(),
            It.Is<List<IncludeOperation<Customer>>>(y => y.Count == 2),
            false,
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Customer>()); // No customers found

        // Act
        await _customerNotificationSettingsResolver.Resolve(orders, country);

        // Assert
        orders[0].SendCanceledOrderNotification.ShouldBe(true); // Should default to true
    }
}