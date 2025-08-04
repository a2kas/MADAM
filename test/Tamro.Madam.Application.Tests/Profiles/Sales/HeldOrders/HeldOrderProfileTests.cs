using System.Reflection;
using AutoMapper;
using NUnit.Framework;
using Shouldly;
using Tamro.Madam.Application.Profiles.Sales.HeldOrders;
using Tamro.Madam.Models.Sales.HeldOrders;
using Tamro.Madam.Repository.Entities.Sales.HeldOrders;

namespace Tamro.Madam.Application.Tests.Profiles.Sales.HeldOrders;

[TestFixture]
public class HeldOrderProfileTests
{
    private IMapper _mapper;

    [SetUp]
    public void SetUp()
    {
        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddMaps(Assembly.GetAssembly(typeof(HeldOrderProfile)))));
    }

    [TestCase(E1HeldNotificationStatusModel.NotSent)]
    [TestCase(E1HeldNotificationStatusModel.Sent)]
    [TestCase(E1HeldNotificationStatusModel.WillNotBeSent)]
    [TestCase(E1HeldNotificationStatusModel.FailureSending)]
    public void E1HeldOrder_To_E1HeldOrderModel_MapsNotificationStatusesCorrectly(E1HeldNotificationStatusModel status)
    {
        // Arrange
        var source = new E1HeldOrder
        {
            NotificationStatus = status,
        };

        // Act
        var dest = _mapper.Map<E1HeldOrderModel>(source);

        // Assert
        dest.NotificationStatus.ShouldBe(status);
        dest.OldNotificationStatus.ShouldBe(status);
    }

    [Test]
    public void E1HeldOrder_To_E1HeldOrderModel_MapsNotificationSendDatesCorrectly()
    {
        // Arrange
        var now = DateTime.UtcNow;
        var source = new E1HeldOrder
        {
            NotificationSendDate = now,
        };

        // Act
        var dest = _mapper.Map<E1HeldOrderModel>(source);

        // Assert
        dest.NotificationSendDate.ShouldBe(now);
        dest.OldNotificationSendDate.ShouldBe(now);
    }
}
