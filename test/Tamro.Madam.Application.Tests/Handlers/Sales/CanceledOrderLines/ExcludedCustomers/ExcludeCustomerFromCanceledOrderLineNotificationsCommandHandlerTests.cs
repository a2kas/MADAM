using Moq;
using NUnit.Framework;
using System.Linq.Expressions;
using Tamro.Madam.Application.Commands.Sales.CanceledOrderLines.ExcludedCustomers;
using Tamro.Madam.Application.Handlers.Sales.CanceledOrderLines.ExcludedCustomers;
using Tamro.Madam.Models.General;
using Tamro.Madam.Models.Sales.CanceledOrderLines.ExcludedCustomers;
using Tamro.Madam.Repository.Common;
using Tamro.Madam.Repository.Entities.Customers;
using Tamro.Madam.Repository.Repositories.Customers;

namespace Tamro.Madam.Application.Tests.Handlers.Sales.CanceledOrderLines.ExcludedCustomers;

[TestFixture]
public class ExcludeCustomerFromCanceledOrderLineNotificationsCommandHandlerTests
{
    private MockRepository _mockRepository;

    private Mock<ICustomerLegalEntityRepository> _customerLegalEntityRepository;

    private ExcludeCustomerFromCanceledOrderLineNotificationsCommandHandler _excludeCustomerFromCanceledOrderLineNotificationsCommandHandler;

    [SetUp]
    public void SetUp()
    {
        _mockRepository = new MockRepository(MockBehavior.Loose);

        _customerLegalEntityRepository = _mockRepository.Create<ICustomerLegalEntityRepository>();

        _excludeCustomerFromCanceledOrderLineNotificationsCommandHandler = new ExcludeCustomerFromCanceledOrderLineNotificationsCommandHandler(_customerLegalEntityRepository.Object);
    }

    [Test]
    public async Task Handle_CustomerExists_SetsSendCanceledOrderNotificationAsFalse()
    {
        // Arrange
        var request = new ExcludeCustomerFromCanceledOrderLineNotificationsCommand(new ExcludedCustomerDetailsModel()
        {
            Customer = new(),
            Country = BalticCountry.EE,
        });

        _customerLegalEntityRepository.Setup(x => x.Get(It.IsAny<Expression<Func<CustomerLegalEntity, bool>>>(), It.IsAny<List<IncludeOperation<CustomerLegalEntity>>>(), true, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new CustomerLegalEntity()
            {
                Id = 6,
                NotificationSettings = new()
                {
                    SendCanceledOrderNotification = true,
                },
                Country = BalticCountry.EE,
            });
        _customerLegalEntityRepository.Setup(x => x.Upsert(It.IsAny<CustomerLegalEntity>(), CancellationToken.None)).ReturnsAsync(new CustomerLegalEntity());

        // Act
        await _excludeCustomerFromCanceledOrderLineNotificationsCommandHandler.Handle(request, CancellationToken.None);

        // Assert
        _customerLegalEntityRepository.Verify(x => x.Upsert(It.Is<CustomerLegalEntity>(y => !y.NotificationSettings.SendCanceledOrderNotification && y.Country == BalticCountry.EE), CancellationToken.None), Times.Once);
    }

    [Test]
    public async Task Handle_CustomerDoesNotExist_CreatesAnActiveLegalEntityWithNotificationSettings_AndSetsCanceledOrderNotificationAsFalse()
    {
        // Arrange
        var request = new ExcludeCustomerFromCanceledOrderLineNotificationsCommand(new ExcludedCustomerDetailsModel()
        {
            Customer = new()
            {
                AddressNumber = 2345,
            },
            Country = BalticCountry.LT,
        });
        _customerLegalEntityRepository.Setup(x => x.Upsert(It.IsAny<CustomerLegalEntity>(), CancellationToken.None)).ReturnsAsync(new CustomerLegalEntity());

        // Act
        await _excludeCustomerFromCanceledOrderLineNotificationsCommandHandler.Handle(request, CancellationToken.None);

        // Assert
        _customerLegalEntityRepository.Verify(x => x.Upsert(It.Is<CustomerLegalEntity>(y => y.E1SoldTo == 2345 && y.Country == BalticCountry.LT && y.IsActive && !y.NotificationSettings.SendCanceledOrderNotification), CancellationToken.None), Times.Once);
    }

    [Test]
    public async Task Handle_CustomerExists_ButNotificationSettingsDoesNotExist_SetsSendCanceledOrderNotificationAsFalse()
    {
        // Arrange
        var request = new ExcludeCustomerFromCanceledOrderLineNotificationsCommand(new ExcludedCustomerDetailsModel()
        {
            Customer = new(),
            Country = BalticCountry.LT,
        });
        _customerLegalEntityRepository.Setup(x => x.Upsert(It.IsAny<CustomerLegalEntity>(), CancellationToken.None)).ReturnsAsync(new CustomerLegalEntity());

        _customerLegalEntityRepository.Setup(x => x.Get(It.IsAny<Expression<Func<CustomerLegalEntity, bool>>>(), It.IsAny<List<IncludeOperation<CustomerLegalEntity>>>(), true, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new CustomerLegalEntity()
            {
                Id = 6,
            });

        // Act
        await _excludeCustomerFromCanceledOrderLineNotificationsCommandHandler.Handle(request, CancellationToken.None);

        // Assert
        _customerLegalEntityRepository.Verify(x => x.Upsert(It.Is<CustomerLegalEntity>(y => !y.NotificationSettings.SendCanceledOrderNotification && y.Country == BalticCountry.LT), CancellationToken.None), Times.Once);
    }
}