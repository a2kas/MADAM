using Moq;
using NUnit.Framework;
using Tamro.Madam.Application.Commands.Sales.CanceledOrderLines.ExcludedCustomers;
using Tamro.Madam.Application.Handlers.Sales.CanceledOrderLines.ExcludedCustomers;
using Tamro.Madam.Repository.Repositories.Customers.Notifications;

namespace Tamro.Madam.Application.Tests.Handlers.Sales.CanceledOrderLines.ExcludedCustomers;

[TestFixture]
public class RemoveCustomerFromExcludedCanceledOrderLineNotificationsCommandHandlerTests
{
    private MockRepository _mockRepository;

    private Mock<ICustomerLegalEntityNotificationRepository> _customerLegalEntityNotificationRepository;

    private RemoveCustomerFromExcludedCanceledOrderLineNotificationsCommandHandler _removeCustomerFromExcludedCanceledOrderLineNotificationsCommandHandler;

    [SetUp]
    public void SetUp()
    {
        _mockRepository = new MockRepository(MockBehavior.Loose);

        _customerLegalEntityNotificationRepository = _mockRepository.Create<ICustomerLegalEntityNotificationRepository>();

        _removeCustomerFromExcludedCanceledOrderLineNotificationsCommandHandler = new RemoveCustomerFromExcludedCanceledOrderLineNotificationsCommandHandler(_customerLegalEntityNotificationRepository.Object);
    }

    [Test]
    public async Task Handle_MarksCustomersWith_SendNotification()
    {
        // Arrange
        var request = new RemoveCustomerFromExcludedCanceledOrderLineNotificationsCommand([6]);

        // Act
        await _removeCustomerFromExcludedCanceledOrderLineNotificationsCommandHandler.Handle(request, CancellationToken.None);

        // Assert
        _customerLegalEntityNotificationRepository.Verify(x => x.MarkSendCanceledOrderNotification(It.Is<List<int>>(y => y.Count == 1 && y.First() == 6), true, CancellationToken.None), Times.Once);
    }
}