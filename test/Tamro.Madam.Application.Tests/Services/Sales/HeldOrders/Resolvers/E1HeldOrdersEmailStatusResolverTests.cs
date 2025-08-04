using Moq;
using NUnit.Framework;
using Shouldly;
using Tamro.Madam.Application.Services.Sales.HeldOrders.Resolvers;
using Tamro.Madam.Models.Sales.HeldOrders;

namespace Tamro.Madam.Application.Tests.Services.Sales.HeldOrders.Resolvers;

[TestFixture]
public class E1HeldOrdersEmailStatusResolverTests
{
    private Mock<TimeProvider> _timeProvider;

    private E1HeldOrdersEmailStatusResolver _e1HeldOrdersEmailStatusResolver;

    [SetUp]
    public void SetUp()
    {
        _timeProvider = new Mock<TimeProvider>();

        _e1HeldOrdersEmailStatusResolver = new E1HeldOrdersEmailStatusResolver(_timeProvider.Object);
    }

    [Test]
    public void ResolveNotifcationStatus_ResolversNotificationStatusesCorrectly()
    {
        // Arrange
        var now = DateTime.UtcNow;
        _timeProvider.Setup(x => x.GetUtcNow()).Returns(now);
        var orders = new List<E1HeldOrderModel>
        {
            new E1HeldOrderModel
            {
                Id = 1,
                HasValidCustomerEmails = true,
                CreatedDate = now,
                NotificationStatus = E1HeldNotificationStatusModel.FailureSending,
                OldNotificationStatus = E1HeldNotificationStatusModel.FailureSending,
            },
            new E1HeldOrderModel
            {
                Id = 2,
                HasValidCustomerEmails = false,
                CreatedDate = now,
                NotificationStatus = E1HeldNotificationStatusModel.FailureSending,
                OldNotificationStatus = E1HeldNotificationStatusModel.FailureSending,
            },
            new E1HeldOrderModel
            {
                Id = 3,
                HasValidCustomerEmails = false,
                CreatedDate = now.AddDays(-6),
                NotificationStatus = E1HeldNotificationStatusModel.FailureSending,
                OldNotificationStatus = E1HeldNotificationStatusModel.FailureSending,
            },
             new E1HeldOrderModel
            {
                Id = 4,
                HasValidCustomerEmails = false,
                CreatedDate = now.AddDays(-6),
                NotificationStatus = E1HeldNotificationStatusModel.NotSent,
                OldNotificationStatus = E1HeldNotificationStatusModel.NotSent,
            },
            new E1HeldOrderModel
            {
                Id = 5,
                HasValidCustomerEmails = false,
                CreatedDate = now.AddDays(-6),
                NotificationStatus = E1HeldNotificationStatusModel.FailureSending,
                OldNotificationStatus = E1HeldNotificationStatusModel.NotSent,
            },
        };

        // Act
        _e1HeldOrdersEmailStatusResolver.ResolveNotifcationStatus(orders);

        // Assert
        orders[0].NotificationStatus.ShouldBe(E1HeldNotificationStatusModel.FailureSending);
        orders[1].NotificationStatus.ShouldBe(E1HeldNotificationStatusModel.FailureSending);
        orders[2].NotificationStatus.ShouldBe(E1HeldNotificationStatusModel.WillNotBeSent);
        orders[3].NotificationStatus.ShouldBe(E1HeldNotificationStatusModel.FailureSending);
    }
}
