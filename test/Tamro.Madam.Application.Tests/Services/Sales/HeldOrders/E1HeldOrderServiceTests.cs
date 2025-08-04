using AutoFixture;
using Moq;
using NUnit.Framework;
using Tamro.Madam.Application.Services.Sales.HeldOrders;
using Tamro.Madam.Models.General;
using Tamro.Madam.Models.Sales.HeldOrders;
using Tamro.Madam.Repository.Repositories.Sales.HeldOrders;

namespace Tamro.Madam.Application.Tests.Services.Sales.HeldOrders;

[TestFixture]
public class E1HeldOrderServiceTests
{
    private Fixture _fixture;

    private Mock<IE1HeldOrderRepository> _e1HeldOrderRepository;

    private E1HeldOrderService _e1HeldOrderService;

    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture();

        _e1HeldOrderRepository = new Mock<IE1HeldOrderRepository>();

        _e1HeldOrderService = new E1HeldOrderService(_e1HeldOrderRepository.Object);
    }

    [Test]
    public async Task GetHeldOrders_GetsHeldOrders()
    {
        // Arrange
        var country = BalticCountry.LV;
        var statuses = _fixture.CreateMany<E1HeldNotificationStatusModel>().ToList();

        // Act
        await _e1HeldOrderService.GetHeldOrders(country, statuses);

        // Assert
        _e1HeldOrderRepository.Verify(x => x.GetHeldOrders(country, statuses), Times.Once);
    }

    [Test]
    public async Task Update_Updates()
    {
        // Arrange
        var order = _fixture.Create<E1HeldOrderModel>();

        // Act
        await _e1HeldOrderService.Update(order);

        // Assert
        _e1HeldOrderRepository.Verify(x => x.Update(order), Times.Once);
    }

    [Test]
    public async Task UpdateMany_UpdatesEachRecord()
    {
        // Arrange
        var orders = _fixture.CreateMany<E1HeldOrderModel>(4).ToList();

        // Act
        await _e1HeldOrderService.UpdateMany(orders);

        // Assert
        _e1HeldOrderRepository.Verify(x => x.Update(It.IsAny<E1HeldOrderModel>()), Times.Exactly(4));
    }
}
