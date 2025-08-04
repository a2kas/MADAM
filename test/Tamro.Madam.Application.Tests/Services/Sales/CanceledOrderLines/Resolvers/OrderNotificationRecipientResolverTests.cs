using AutoFixture;
using Moq;
using NUnit.Framework;
using Shouldly;
using Tamro.Madam.Application.Services.Customers.Factories;
using Tamro.Madam.Application.Services.Sales.CanceledOrderLines.Resolvers;
using Tamro.Madam.Models.Customers.Wholesale;
using Tamro.Madam.Models.General;
using Tamro.Madam.Models.Sales.CanceledOrderLines;
using Tamro.Madam.Repository.Repositories.Customers.Wholesale;

namespace Tamro.Madam.Application.Tests.Services.Sales.CanceledOrderLines.Resolvers;

[TestFixture]
public class OrderNotificationRecipientResolverTests
{
    private Fixture _fixture;

    private Mock<IWholesaleCustomerRepository> _wholesaleCustomerRepository;
    private Mock<IWholesaleCustomerRepositoryFactory> _wholesaleCustomerRepositoryFactory;
    private Mock<TimeProvider> _timeProvider;

    private OrderNotificationRecipientResolver _orderNotificationRecipientResolver;

    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture();

        _wholesaleCustomerRepository = new Mock<IWholesaleCustomerRepository>();
        _wholesaleCustomerRepositoryFactory = new Mock<IWholesaleCustomerRepositoryFactory>();
        _wholesaleCustomerRepositoryFactory.Setup(x => x.Get(It.IsAny<BalticCountry>())).Returns(_wholesaleCustomerRepository.Object);
        _timeProvider = new Mock<TimeProvider>();
        _timeProvider.Setup(x => x.GetUtcNow()).Returns(DateTime.UtcNow);

        _orderNotificationRecipientResolver = new OrderNotificationRecipientResolver(_wholesaleCustomerRepositoryFactory.Object, _timeProvider.Object);
    }

    [Test]
    public async Task OrderNotificationRecipientResolver_PriorityShouldBeEqualTo1()
    {
        // Assert
        _orderNotificationRecipientResolver.Priority.ShouldBe(1);
    }

    [Test]
    public async Task Resolve_NotificationRecipientsByDistinctAddressNumbers()
    {
        // Arrange
        var country = BalticCountry.LV;
        var orders = _fixture.CreateMany<CanceledOrderHeaderModel>().ToList();
        orders[0].E1ShipTo = 1;
        orders[1].E1ShipTo = 2;
        orders[2].E1ShipTo = 1;
        _wholesaleCustomerRepository.Setup(x => x.GetMany(It.IsAny<WholesaleCustomerSearchModel>())).ReturnsAsync([]);

        // Act
        await _orderNotificationRecipientResolver.Resolve(orders, country);

        // Assert
        _wholesaleCustomerRepository.Verify(x => x.GetMany(It.Is<WholesaleCustomerSearchModel>(
            y => y.AddressNumbers != null &&
                 y.AddressNumbers.Count == 2 &&
                 y.AddressNumbers.Contains(1) &&
                 y.AddressNumbers.Contains(2)))
        , Times.Once);
    }

    [Test]
    public async Task Resolve_ResolvesSoldTo()
    {
        // Arrange
        var country = BalticCountry.LV;
        var orders = _fixture.CreateMany<CanceledOrderHeaderModel>(2).ToList();
        orders[0].E1ShipTo = 1;
        orders[0].SoldTo = default;
        orders[1].E1ShipTo = 2;
        orders[1].SoldTo = default;
        var customers = _fixture.CreateMany<WholesaleCustomerModel>().ToList();
        customers[0].AddressNumber = 2;
        customers[1].AddressNumber = 1;
        customers[2].AddressNumber = 3;
        _wholesaleCustomerRepository.Setup(x => x.GetMany(It.IsAny<WholesaleCustomerSearchModel>())).ReturnsAsync(customers);

        // Act
        await _orderNotificationRecipientResolver.Resolve(orders, country);

        // Assert
        orders[0].SoldTo.ShouldBe(customers[1].LegalEntityNumber);
        orders[1].SoldTo.ShouldBe(customers[0].LegalEntityNumber);
    }

    [Test]
    public async Task Resolve_NoValidEmail_SetsEmailStatusCorrectly()
    {
        // Arrange
        var country = BalticCountry.LV;
        var orders = _fixture.CreateMany<CanceledOrderHeaderModel>(5).ToList();
        orders.ForEach(x =>
        {
            x.Lines.ToList().ForEach(y =>
            {
                y.EmailStatus = CanceledOrderLineEmailStatus.NotSent;
                y.CreatedDate = DateTime.UtcNow;
                y.RowVer = DateTime.UtcNow;
            });
        });
        orders[0].E1ShipTo = 1;
        orders[0].SoldTo = default;
        orders[1].E1ShipTo = 2;
        orders[1].SoldTo = default;
        orders[1].Lines.ToList().ForEach(x => x.CreatedDate = DateTime.UtcNow.AddDays(-4));
        orders[2].E1ShipTo = 3;
        orders[2].SoldTo = default;
        orders[3].E1ShipTo = 4;
        orders[3].SoldTo = default;
        orders[4].E1ShipTo = 5;
        orders[4].SoldTo = default;
        var customers = _fixture.CreateMany<WholesaleCustomerModel>(5).ToList();
        customers[0].AddressNumber = 100;
        customers[1].AddressNumber = 1;
        customers[1].EmailAddress = "";
        customers[2].AddressNumber = 3;
        customers[2].EmailAddress = "some@email.com, another@emailsome.com";
        customers[3].AddressNumber = 4;
        customers[3].EmailAddress = "someemail@email.com, some(email)_some14@email.com";
        customers[4].AddressNumber = 5;
        customers[4].EmailAddress = "ome(email)_some14@email.com";
        orders[4].Lines.ToList().ForEach(x =>
        {
            x.CreatedDate = DateTime.UtcNow.AddDays(-3).AddHours(-1);
            x.EmailStatus = CanceledOrderLineEmailStatus.FailureSending;
        });
        _wholesaleCustomerRepository.Setup(x => x.GetMany(It.IsAny<WholesaleCustomerSearchModel>())).ReturnsAsync(customers);

        // Act
        await _orderNotificationRecipientResolver.Resolve(orders, country);

        // Assert
        orders[0].Lines.Any(x => x.EmailStatus == CanceledOrderLineEmailStatus.FailureSending).ShouldBeTrue();
        orders[1].Lines.Any(x => x.EmailStatus == CanceledOrderLineEmailStatus.FailureSending).ShouldBeTrue();
        orders[2].Lines.Any(x => x.EmailStatus == CanceledOrderLineEmailStatus.NotSent).ShouldBeTrue();
        orders[3].Lines.Any(x => x.EmailStatus == CanceledOrderLineEmailStatus.FailureSending).ShouldBeTrue();
        orders[4].Lines.Any(x => x.EmailStatus == CanceledOrderLineEmailStatus.WillNotBeSent).ShouldBeTrue();
    }

    [TestCase("", "")]
    [TestCase(" ", "")]
    [TestCase(" someemail@email.com", "someemail@email.com")]
    [TestCase("some@email.com, another@emailsome.com ", "some@email.com,another@emailsome.com")]
    [TestCase("some@email.com, tiina.salusaar€mail.ee,another@emailsome.com ", "some@email.com,another@emailsome.com")]
    [TestCase(" tiina.salusaar€mail.ee", "")]
    [TestCase("some@email.com, tiina.salusaarmail.ee", "some@email.com")]
    [TestCase("tiina.salusaar#mail.ee, tiina.salusaarmail.ee", "")]
    public async Task Resolve_SanitizesEmailAddresses(string emailAddress, string expectedEmailAddress)
    {
        // Arrange
        var country = BalticCountry.LV;
        var orders = _fixture.Build<CanceledOrderHeaderModel>()
            .With(x => x.E1ShipTo, 1)
            .With(x => x.Lines, _fixture.Build<CanceledOrderLineModel>()
                .With(x => x.EmailStatus, CanceledOrderLineEmailStatus.NotSent)
                .With(x => x.CreatedDate, DateTime.UtcNow)
                .With(x => x.RowVer, DateTime.UtcNow)
                .CreateMany(1).ToList())
            .CreateMany(1).ToList();

        var customers = _fixture.CreateMany<WholesaleCustomerModel>(2).ToList();
        customers[0].AddressNumber = 1;
        customers[0].EmailAddress = emailAddress;
        customers[1].AddressNumber = 2;

        _wholesaleCustomerRepository.Setup(x => x.GetMany(It.IsAny<WholesaleCustomerSearchModel>())).ReturnsAsync(customers);

        // Act
        await _orderNotificationRecipientResolver.Resolve(orders, country);

        // Assert
        var resultEmailAddress = orders[0].Lines.ElementAt(0).EmailAddress;
        resultEmailAddress.ShouldBe(expectedEmailAddress);
    }
}
