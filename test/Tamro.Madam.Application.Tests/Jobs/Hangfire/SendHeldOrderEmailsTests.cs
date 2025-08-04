using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Shouldly;
using Tamro.Madam.Application.Jobs.Hangfire;
using Tamro.Madam.Application.Services.Sales.HeldOrders;
using Tamro.Madam.Application.Services.Sales.HeldOrders.Resolvers;
using Tamro.Madam.Models.General;
using Tamro.Madam.Models.Sales.HeldOrders;
using Tamro.Madam.Repository.Context.E1Gateway;
using TamroUtilities.EFCore.UnitOfWork;
using TamroUtilities.Hangfire.Models;

namespace Tamro.Madam.Application.Tests.Jobs.Hangfire;

[TestFixture]
public class SendHeldOrderEmailsTests
{
    private Fixture _fixture;

    private Mock<IE1HeldOrdersResolver> _e1HeldOrdersResolver1;
    private Mock<IE1HeldOrdersResolver> _e1HeldOrdersResolver2;

    private IEnumerable<IE1HeldOrdersResolver> _resolvers;
    private Mock<IE1HeldOrderService> _e1HeldOrderService;
    private Mock<IE1HeldOrderEmailService> _e1HeldOrderEmailService;
    private Mock<IE1HeldOrdersEmailStatusResolver> _e1HeldOrdersEmailStatusResolver;
    private Mock<TimeProvider> _timeProvider;
    private Mock<IUnitOfWork<E1GatewayDbContext>> _unitOfWork;
    private Mock<ILogger<SendHeldOrderEmails>> _logger;

    private SendHeldOrderEmails _sendHeldOrderEmails;

    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture();

        _e1HeldOrdersResolver1 = new Mock<IE1HeldOrdersResolver>();
        _e1HeldOrdersResolver2 = new Mock<IE1HeldOrdersResolver>();

        _resolvers = [_e1HeldOrdersResolver1.Object, _e1HeldOrdersResolver2.Object];
        _e1HeldOrderService = new Mock<IE1HeldOrderService>();
        _e1HeldOrderEmailService = new Mock<IE1HeldOrderEmailService>();
        _e1HeldOrdersEmailStatusResolver = new Mock<IE1HeldOrdersEmailStatusResolver>();
        _timeProvider = new Mock<TimeProvider>();
        _unitOfWork = new Mock<IUnitOfWork<E1GatewayDbContext>>();
        _logger = new Mock<ILogger<SendHeldOrderEmails>>();

        _sendHeldOrderEmails = new SendHeldOrderEmails(
            _resolvers,
            _e1HeldOrderService.Object,
            _e1HeldOrderEmailService.Object,
            _e1HeldOrdersEmailStatusResolver.Object,
            _timeProvider.Object,
            _unitOfWork.Object,
            _logger.Object);
    }

    [Test]
    public void SendHeldOrderEmails_IsHangfireJob()
    {
        // Assert
        _sendHeldOrderEmails.ShouldBeAssignableTo<HangfireJobBase>();
    }

    [Test]
    public async Task JobToRun_GetHeldOrdersForLv()
    {
        // Act
        await _sendHeldOrderEmails.JobToRun(null, null);

        // Assert
        _e1HeldOrderService.Verify(x => x.GetHeldOrders(BalticCountry.LV, new List<E1HeldNotificationStatusModel> { E1HeldNotificationStatusModel.NotSent, E1HeldNotificationStatusModel.FailureSending }), Times.Once);
    }

    [Test]
    public async Task JobToRun_NoCanceledOrders_DoesNotProcess()
    {
        // Act
        await _sendHeldOrderEmails.JobToRun(null, null);

        // Assert
        _e1HeldOrdersResolver1.VerifyNoOtherCalls();
        _e1HeldOrdersResolver2.VerifyNoOtherCalls();
    }

    [Test]
    public async Task JobToRun_ProcessAllResolvers()
    {
        // Arrange
        var country = BalticCountry.LV;
        var heldOrders = _fixture.CreateMany<E1HeldOrderModel>();
        _e1HeldOrderService.Setup(x => x.GetHeldOrders(BalticCountry.LV, It.IsAny<List<E1HeldNotificationStatusModel>>())).ReturnsAsync(heldOrders);

        // Act
        await _sendHeldOrderEmails.JobToRun(null, null);

        // Assert
        _e1HeldOrdersResolver1.Verify(x => x.Resolve(heldOrders, country), Times.Once);
        _e1HeldOrdersResolver2.Verify(x => x.Resolve(heldOrders, country), Times.Once);
    }

    [Test]
    public async Task JobToRun_ResolvesEmailValidity()
    {
        // Arrange
        var country = BalticCountry.LV;
        var heldOrders = _fixture.CreateMany<E1HeldOrderModel>();
        _e1HeldOrderService.Setup(x => x.GetHeldOrders(BalticCountry.LV, It.IsAny<List<E1HeldNotificationStatusModel>>())).ReturnsAsync(heldOrders);

        // Act
        await _sendHeldOrderEmails.JobToRun(null, null);

        // Assert
        _e1HeldOrderEmailService.Verify(x => x.ResolveEmailValidity(heldOrders), Times.Once);
    }

    [Test]
    public async Task JobToRun_SendEmailsToCustomersAndUpdateStatuses_ProcessesOrdersWithValidEmails()
    {
        // Arrange
        var country = BalticCountry.LV;
        var heldOrders = _fixture.CreateMany<E1HeldOrderModel>().ToList();
        heldOrders[0].NotificationStatus = E1HeldNotificationStatusModel.NotSent;
        heldOrders[0].OldNotificationStatus = E1HeldNotificationStatusModel.NotSent;
        heldOrders[0].HasValidCustomerEmails = false;
        heldOrders[1].NotificationStatus = E1HeldNotificationStatusModel.NotSent;
        heldOrders[1].OldNotificationStatus = E1HeldNotificationStatusModel.NotSent;
        heldOrders[1].HasValidCustomerEmails = true;
        heldOrders[2].NotificationStatus = E1HeldNotificationStatusModel.NotSent;
        heldOrders[2].OldNotificationStatus = E1HeldNotificationStatusModel.NotSent;
        heldOrders[2].HasValidCustomerEmails = false;
        _e1HeldOrderService.Setup(x => x.GetHeldOrders(BalticCountry.LV, It.IsAny<List<E1HeldNotificationStatusModel>>())).ReturnsAsync(heldOrders);

        // Act
        await _sendHeldOrderEmails.JobToRun(null, null);

        // Assert

        //TODO: revert OTHERS-2703 when Held orders functionality ready to start to work
        //_e1HeldOrderEmailService.Verify(x => x.SendCustomerEmail(It.Is<E1HeldOrderModel>(x => x.Id == heldOrders[1].Id), country), Times.Once);
    }

    [Test]
    public async Task JobToRun_SendsEmailsOnlyForSuccessfullyUpdatedOrdersAndUpdatesOrderData()
    {
        // Arrange
        var country = BalticCountry.LV;
        var heldOrders = _fixture.CreateMany<E1HeldOrderModel>().ToList();
        heldOrders[0].NotificationSendDate = default;
        heldOrders[0].OldNotificationSendDate = default;
        heldOrders[0].NotificationStatus = E1HeldNotificationStatusModel.NotSent;
        heldOrders[0].OldNotificationStatus = E1HeldNotificationStatusModel.NotSent;
        heldOrders[0].HasValidCustomerEmails = true;
        heldOrders[1].NotificationSendDate = default;
        heldOrders[1].OldNotificationSendDate = default;
        heldOrders[1].NotificationStatus = E1HeldNotificationStatusModel.NotSent;
        heldOrders[1].OldNotificationStatus = E1HeldNotificationStatusModel.NotSent;
        heldOrders[1].HasValidCustomerEmails = true;
        heldOrders[2].NotificationSendDate = default;
        heldOrders[2].OldNotificationSendDate = default;
        heldOrders[2].NotificationStatus = E1HeldNotificationStatusModel.NotSent;
        heldOrders[2].OldNotificationStatus = E1HeldNotificationStatusModel.NotSent;
        heldOrders[2].HasValidCustomerEmails = false;
        _e1HeldOrderService.Setup(x => x.GetHeldOrders(BalticCountry.LV, It.IsAny<List<E1HeldNotificationStatusModel>>())).ReturnsAsync(heldOrders);
        _e1HeldOrderService.SetupSequence(x => x.Update(It.IsAny<E1HeldOrderModel>()))
            .ThrowsAsync(new Exception())
            .ReturnsAsync(new E1HeldOrderModel());
        var now = DateTime.UtcNow;
        _timeProvider.Setup(x => x.GetUtcNow()).Returns(now);

        // Act
        await _sendHeldOrderEmails.JobToRun(null, null);

        // Assert

        //TODO: revert OTHERS-2703 when Held orders functionality ready to start to work
        //_e1HeldOrderEmailService.Verify(x => x.SendCustomerEmail(It.IsAny<E1HeldOrderModel>(), country), Times.Once);
        heldOrders[0].NotificationStatus.ShouldBe(E1HeldNotificationStatusModel.NotSent);
        heldOrders[0].NotificationSendDate.ShouldBeNull();
        heldOrders[1].NotificationStatus.ShouldBe(E1HeldNotificationStatusModel.Sent);
        heldOrders[1].NotificationSendDate.ShouldBe(now);
        heldOrders[2].NotificationStatus.ShouldBe(E1HeldNotificationStatusModel.NotSent);
        heldOrders[2].NotificationSendDate.ShouldBeNull();
    }

    [Test]
    public async Task JobToRun_UpdatesOrdersOnlyForOrdersWithSuccessfullySentEmails()
    {
        // Arrange
        var country = BalticCountry.LV;
        var heldOrders = _fixture.CreateMany<E1HeldOrderModel>().ToList();
        heldOrders[0].NotificationStatus = E1HeldNotificationStatusModel.NotSent;
        heldOrders[0].OldNotificationStatus = E1HeldNotificationStatusModel.NotSent;
        heldOrders[0].HasValidCustomerEmails = true;
        heldOrders[1].NotificationStatus = E1HeldNotificationStatusModel.NotSent;
        heldOrders[1].OldNotificationStatus = E1HeldNotificationStatusModel.NotSent;
        heldOrders[1].HasValidCustomerEmails = true;
        heldOrders[2].NotificationStatus = E1HeldNotificationStatusModel.NotSent;
        heldOrders[2].OldNotificationStatus = E1HeldNotificationStatusModel.NotSent;
        heldOrders[2].HasValidCustomerEmails = false;
        _e1HeldOrderService.Setup(x => x.GetHeldOrders(BalticCountry.LV, It.IsAny<List<E1HeldNotificationStatusModel>>())).ReturnsAsync(heldOrders);
        _e1HeldOrderEmailService.SetupSequence(x => x.SendCustomerEmail(It.IsAny<E1HeldOrderModel>(), country))
            .Returns(Task.CompletedTask)
            .ThrowsAsync(new Exception());
        _e1HeldOrderService.SetupSequence(x => x.Update(It.IsAny<E1HeldOrderModel>()))
            .ThrowsAsync(new Exception())
            .ReturnsAsync(new E1HeldOrderModel());

        // Act
        await _sendHeldOrderEmails.JobToRun(null, null);

        // Assert
        _unitOfWork.Verify(x => x.CommitAsync(), Times.Once);
        _unitOfWork.Verify(x => x.RollbackAsync(), Times.Once);
    }

    [Test]
    public async Task JobToRun_NotifyAboutInvalidEmails()
    {
        // Arrange
        var country = BalticCountry.LV;
        var heldOrders = _fixture.CreateMany<E1HeldOrderModel>().ToList();
        heldOrders[0].NotificationStatus = E1HeldNotificationStatusModel.FailureSending;
        heldOrders[0].OldNotificationStatus = E1HeldNotificationStatusModel.NotSent;
        heldOrders[0].HasValidCustomerEmails = false;
        heldOrders[1].NotificationStatus = E1HeldNotificationStatusModel.NotSent;
        heldOrders[1].OldNotificationStatus = E1HeldNotificationStatusModel.NotSent;
        heldOrders[1].HasValidCustomerEmails = false;
        heldOrders[2].NotificationStatus = E1HeldNotificationStatusModel.FailureSending;
        heldOrders[2].OldNotificationStatus = E1HeldNotificationStatusModel.FailureSending;
        heldOrders[2].HasValidCustomerEmails = false;
        _e1HeldOrderService.Setup(x => x.GetHeldOrders(BalticCountry.LV, It.IsAny<List<E1HeldNotificationStatusModel>>())).ReturnsAsync(heldOrders);

        // Act
        await _sendHeldOrderEmails.JobToRun(null, null);

        // Assert

        //TODO: revert OTHERS-2703 when Held orders functionality ready to start to work
        //_e1HeldOrderEmailService.Verify(x => x.SendEmployeeEmails(It.Is<IEnumerable<E1HeldOrderModel>>(orders => orders.Count() == 1 && orders.First().Id == heldOrders[0].Id), country), Times.Once);
    }

    [Test]
    public async Task JobToRun_UpdatesMany()
    {
        // Arrange
        var country = BalticCountry.LV;
        var heldOrders = _fixture.CreateMany<E1HeldOrderModel>().ToList();
        heldOrders[0].NotificationStatus = E1HeldNotificationStatusModel.FailureSending;
        heldOrders[0].OldNotificationStatus = E1HeldNotificationStatusModel.NotSent;
        heldOrders[0].HasValidCustomerEmails = true;
        heldOrders[1].NotificationStatus = E1HeldNotificationStatusModel.NotSent;
        heldOrders[1].OldNotificationStatus = E1HeldNotificationStatusModel.NotSent;
        heldOrders[1].HasValidCustomerEmails = false;
        heldOrders[2].NotificationStatus = E1HeldNotificationStatusModel.WillNotBeSent;
        heldOrders[2].OldNotificationStatus = E1HeldNotificationStatusModel.NotSent;
        heldOrders[2].HasValidCustomerEmails = true;
        _e1HeldOrderService.Setup(x => x.GetHeldOrders(BalticCountry.LV, It.IsAny<List<E1HeldNotificationStatusModel>>())).ReturnsAsync(heldOrders);

        // Act
        await _sendHeldOrderEmails.JobToRun(null, null);

        // Assert
        _e1HeldOrderService.Verify(x => x.UpdateMany(It.Is<IEnumerable<E1HeldOrderModel>>(orders => orders.Count() == 1 && orders.First().Id == heldOrders[1].Id)), Times.Once);
    }
}
