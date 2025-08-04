using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Shouldly;
using Tamro.Madam.Application.Jobs.Hangfire;
using Tamro.Madam.Application.Services.Sales.CanceledOrderLines;
using Tamro.Madam.Application.Services.Sales.CanceledOrderLines.Factories;
using Tamro.Madam.Application.Services.Sales.CanceledOrderLines.Resolvers;
using Tamro.Madam.Application.Services.Sales.Decorators;
using Tamro.Madam.Models.Configuration;
using Tamro.Madam.Models.General;
using Tamro.Madam.Models.Sales;
using Tamro.Madam.Models.Sales.CanceledOrderLines;
using Tamro.Madam.Repository.Context.E1Gateway;
using Tamro.Madam.Repository.Repositories.Sales;
using Tamroutilities.Email.Models;
using Tamroutilities.Email.Sender;
using TamroUtilities.EFCore.UnitOfWork;
using TamroUtilities.Hangfire.Models;

namespace Tamro.Madam.Application.Tests.Jobs.Hangfire;

[TestFixture]
public class SendCanceledLineEmailsTests
{
    private Fixture _fixture;

    private Mock<ICanceledOrderLinesResolver> _resolver1;
    private Mock<ICanceledOrderLinesResolver> _resolver2;

    private Mock<ICanceledOrderLinesEmailGenerator> _canceledOrderLinesEmailGenerator;

    private IEnumerable<ICanceledOrderLinesResolver> _resolvers;
    private Mock<IE1CanceledOrderRepository> _canceledOrderRepository;
    private Mock<IUnitOfWork<E1GatewayDbContext>> _unitOfWork;
    private Mock<ICanceledOrderLinesEmailGeneratorFactory> _canceledOrderLinesEmailGeneratorFactory;
    private Mock<IEmailSender> _emailSender;
    private Mock<ISalesOrderCustomerDecorator> _canceledOrderCustomerDecorator;
    private List<CanceledLineSetting> _canceledLineSettings;
    private Mock<ILogger<SendCanceledLineEmails>> _logger;

    private SendCanceledLineEmails _sendCanceledLineEmails;

    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture();

        _resolver1 = new Mock<ICanceledOrderLinesResolver>();
        _resolver1.Setup(x => x.Priority).Returns(1);

        _resolver2 = new Mock<ICanceledOrderLinesResolver>();
        _resolver1.Setup(x => x.Priority).Returns(2);

        _resolvers = [_resolver2.Object, _resolver1.Object];
        _canceledOrderRepository = new Mock<IE1CanceledOrderRepository>();
        _unitOfWork = new Mock<IUnitOfWork<E1GatewayDbContext>>();
        _canceledOrderLinesEmailGeneratorFactory = new Mock<ICanceledOrderLinesEmailGeneratorFactory>();
        _canceledOrderLinesEmailGenerator = new Mock<ICanceledOrderLinesEmailGenerator>();
        _canceledOrderLinesEmailGeneratorFactory.Setup(x => x.Get(It.IsAny<BalticCountry>())).Returns(_canceledOrderLinesEmailGenerator.Object);
        _emailSender = new Mock<IEmailSender>();
        _canceledOrderCustomerDecorator = new Mock<ISalesOrderCustomerDecorator>();
        _canceledLineSettings =
        [
            new CanceledLineSetting
            {
                Country = "LV",
                ResponsiblePerson = "SomePerson1@tamro.com"
            },
            new CanceledLineSetting
            {
                Country = "LT",
                ResponsiblePerson = "SomePerson2@tamro.com"
            }
        ];
        _logger = new Mock<ILogger<SendCanceledLineEmails>>();

        _sendCanceledLineEmails = new SendCanceledLineEmails(
            _resolvers,
            _canceledOrderRepository.Object,
            _unitOfWork.Object,
            _canceledOrderLinesEmailGeneratorFactory.Object,
            _emailSender.Object,
            _canceledOrderCustomerDecorator.Object,
            _canceledLineSettings,
            _logger.Object);
    }

    [Test]
    public void SafetyStockItemsUpdate_IsHangfireJob()
    {
        // Assert
        _sendCanceledLineEmails.ShouldBeAssignableTo<HangfireJobBase>();
    }

    [Test]
    public async Task JobToRun_GetsCanceledOrdersForAllBalticCountries()
    {
        // Act
        await _sendCanceledLineEmails.JobToRun(null, null);

        // Assert
        _canceledOrderRepository.Verify(x => x.GetCanceledOrders(BalticCountry.LT, new List<CanceledOrderLineEmailStatus> { CanceledOrderLineEmailStatus.NotSent, CanceledOrderLineEmailStatus.FailureSending }), Times.Once);
        _canceledOrderRepository.Verify(x => x.GetCanceledOrders(BalticCountry.LV, new List<CanceledOrderLineEmailStatus> { CanceledOrderLineEmailStatus.NotSent, CanceledOrderLineEmailStatus.FailureSending }), Times.Once);
        _canceledOrderRepository.Verify(x => x.GetCanceledOrders(BalticCountry.EE, new List<CanceledOrderLineEmailStatus> { CanceledOrderLineEmailStatus.NotSent, CanceledOrderLineEmailStatus.FailureSending }), Times.Once);
    }

    [Test]
    public async Task JobToRun_NoCanceledOrders_DoesNotProcess()
    {
        // Act
        await _sendCanceledLineEmails.JobToRun(null, null);

        // Assert
        _resolver1.VerifyNoOtherCalls();
        _resolver2.VerifyNoOtherCalls();
    }

    [Test]
    public async Task JobToRun_ProcessAllResolvers()
    {
        // Arrange
        var country = BalticCountry.LV;
        var canceledOrders = _fixture.CreateMany<CanceledOrderHeaderModel>();
        _canceledOrderRepository.Setup(x => x.GetCanceledOrders(country, It.IsAny<List<CanceledOrderLineEmailStatus>>())).ReturnsAsync(canceledOrders);

        // Act
        await _sendCanceledLineEmails.JobToRun(null, null);

        // Assert
        _resolver1.Verify(x => x.Resolve(canceledOrders, country), Times.Once);
        _resolver2.Verify(x => x.Resolve(canceledOrders, country), Times.Once);
    }

    [Test]
    public async Task JobToRun_NoFailureSendingAndHasSendCanceledOrderNotificationFalse_SetsEmailStatusToWillNotBeSent()
    {
        // Arrange
        var country = BalticCountry.LV;
        var canceledOrders = _fixture.CreateMany<CanceledOrderHeaderModel>().ToList();
        canceledOrders[0].Lines.First().EmailStatus = CanceledOrderLineEmailStatus.FailureSending;
        canceledOrders[0].SendCanceledOrderNotification = true;
        canceledOrders[1].Lines.First().EmailStatus = CanceledOrderLineEmailStatus.NotSent;
        canceledOrders[1].SendCanceledOrderNotification = false;
        canceledOrders[2].Lines.First().EmailStatus = CanceledOrderLineEmailStatus.NotSent;
        canceledOrders[2].SendCanceledOrderNotification = true;
        _canceledOrderRepository.Setup(x => x.GetCanceledOrders(country, It.IsAny<List<CanceledOrderLineEmailStatus>>())).ReturnsAsync(canceledOrders);

        // Act
        await _sendCanceledLineEmails.JobToRun(null, null);

        // Assert
        canceledOrders[0].Lines.First().EmailStatus.ShouldNotBe(CanceledOrderLineEmailStatus.WillNotBeSent);
        canceledOrders[1].Lines.All(x => x.EmailStatus == CanceledOrderLineEmailStatus.WillNotBeSent).ShouldBeTrue();
        canceledOrders[2].Lines.First().EmailStatus.ShouldNotBe(CanceledOrderLineEmailStatus.WillNotBeSent);
    }

    [Test]
    public async Task JobToRun_UpdatesOrdersWhichWillBeNotSentStatuses()
    {
        // Arrange
        var country = BalticCountry.LV;
        var canceledOrders = _fixture.CreateMany<CanceledOrderHeaderModel>(5).ToList();
        canceledOrders[0].Lines.ElementAt(0).EmailStatus = CanceledOrderLineEmailStatus.FailureSending;
        canceledOrders[0].Lines.ElementAt(1).EmailStatus = CanceledOrderLineEmailStatus.FailureSending;
        canceledOrders[0].Lines.ElementAt(2).EmailStatus = CanceledOrderLineEmailStatus.FailureSending;
        canceledOrders[0].SendCanceledOrderNotification = false;
        canceledOrders[1].Lines.First().EmailStatus = CanceledOrderLineEmailStatus.NotSent;
        canceledOrders[1].SendCanceledOrderNotification = false;
        canceledOrders[2].Lines.First().EmailStatus = CanceledOrderLineEmailStatus.NotSent;
        canceledOrders[2].SendCanceledOrderNotification = true;
        canceledOrders[3].Lines.First().EmailStatus = CanceledOrderLineEmailStatus.NotSent;
        canceledOrders[3].SendCanceledOrderNotification = true;
        canceledOrders[4].Lines.ElementAt(0).EmailStatus = CanceledOrderLineEmailStatus.FailureSending;
        canceledOrders[4].Lines.ElementAt(1).EmailStatus = CanceledOrderLineEmailStatus.FailureSending;
        canceledOrders[4].Lines.ElementAt(2).EmailStatus = CanceledOrderLineEmailStatus.FailureSending;
        canceledOrders[4].SendCanceledOrderNotification = true;
        _canceledOrderRepository.Setup(x => x.GetCanceledOrders(country, It.IsAny<List<CanceledOrderLineEmailStatus>>())).ReturnsAsync(canceledOrders);

        // Act
        await _sendCanceledLineEmails.JobToRun(null, null);

        // Assert
        canceledOrders[0].Lines.All(x => x.EmailStatus == CanceledOrderLineEmailStatus.WillNotBeSent).ShouldBeTrue();
        canceledOrders[4].Lines.All(x => x.EmailStatus == CanceledOrderLineEmailStatus.FailureSending).ShouldBeTrue();
        _canceledOrderRepository.Verify(x => x.Update(canceledOrders[0]), Times.Once);
        _canceledOrderRepository.Verify(x => x.Update(canceledOrders[4]), Times.Once);
    }

    [Test]
    public async Task JobToRun_SendsEmailsAndUpdatesOrdersWhichAreNotSentAndRecipientHasSendCanceledOrderNotificationSetToTrue()
    {
        // Arrange
        var country = BalticCountry.LT;
        var canceledOrders = _fixture.CreateMany<CanceledOrderHeaderModel>(4).ToList();
        canceledOrders[0].Lines.ElementAt(0).EmailStatus = CanceledOrderLineEmailStatus.FailureSending;
        canceledOrders[0].Lines.ElementAt(1).EmailStatus = CanceledOrderLineEmailStatus.FailureSending;
        canceledOrders[0].Lines.ElementAt(2).EmailStatus = CanceledOrderLineEmailStatus.FailureSending;
        canceledOrders[0].SendCanceledOrderNotification = false;
        canceledOrders[1].Lines.First().EmailStatus = CanceledOrderLineEmailStatus.NotSent;
        canceledOrders[1].Lines.First().NotificationSendDate = null;
        canceledOrders[1].SendCanceledOrderNotification = false;
        canceledOrders[2].Lines.First().EmailStatus = CanceledOrderLineEmailStatus.NotSent;
        canceledOrders[2].SendCanceledOrderNotification = true;
        canceledOrders[2].Lines.First().NotificationSendDate = null;
        canceledOrders[3].Lines.First().EmailStatus = CanceledOrderLineEmailStatus.NotSent;
        canceledOrders[3].SendCanceledOrderNotification = true;
        canceledOrders[3].Lines.First().NotificationSendDate = null;
        _canceledOrderRepository.Setup(x => x.GetCanceledOrders(country, It.IsAny<List<CanceledOrderLineEmailStatus>>())).ReturnsAsync(canceledOrders);
        _canceledOrderLinesEmailGenerator.Setup(x => x.GenerateCanceledLinesEmail(It.IsAny<IGrouping<int, CanceledOrderHeaderModel>>())).Returns(new Email { Receivers = ["test1@email.com"] });

        // Act
        await _sendCanceledLineEmails.JobToRun(null, null);

        // Assert
        _canceledOrderLinesEmailGenerator.Verify(x => x.GenerateCanceledLinesEmail(It.IsAny<IGrouping<int, CanceledOrderHeaderModel>>()), Times.Exactly(2));
        canceledOrders[1].Lines.ElementAt(0).NotificationSendDate.ShouldBeNull();
        canceledOrders[2].Lines.All(x => x.EmailStatus == CanceledOrderLineEmailStatus.Sent).ShouldBeTrue();
        canceledOrders[2].Lines.ElementAt(0).NotificationSendDate.ShouldNotBeNull();
        canceledOrders[3].Lines.All(x => x.EmailStatus == CanceledOrderLineEmailStatus.Sent).ShouldBeTrue();
        canceledOrders[3].Lines.ElementAt(0).NotificationSendDate.ShouldNotBeNull();
        _canceledOrderRepository.Verify(x => x.Update(canceledOrders[2]), Times.Once);
        _canceledOrderRepository.Verify(x => x.Update(canceledOrders[3]), Times.Once);
        _emailSender.Verify(x => x.SendEmailAsync(It.IsAny<Email>()), Times.Exactly(2));
    }

    [Test]
    public async Task JobToRun_SendsEmailOnlyForSuccessfullyUpdatedOrders()
    {
        // Arrange
        var country = BalticCountry.LT;
        var canceledOrders = _fixture.CreateMany<CanceledOrderHeaderModel>(2).ToList();
        canceledOrders[0].Lines.First().EmailStatus = CanceledOrderLineEmailStatus.NotSent;
        canceledOrders[0].SendCanceledOrderNotification = true;
        canceledOrders[1].Lines.First().EmailStatus = CanceledOrderLineEmailStatus.NotSent;
        canceledOrders[1].SendCanceledOrderNotification = true;
        _canceledOrderRepository.Setup(x => x.GetCanceledOrders(country, It.IsAny<List<CanceledOrderLineEmailStatus>>())).ReturnsAsync(canceledOrders);
        _canceledOrderRepository.SetupSequence(x => x.Update(It.IsAny<CanceledOrderHeaderModel>()))
            .ThrowsAsync(new Exception())
            .ReturnsAsync(canceledOrders[1]);
        _canceledOrderLinesEmailGenerator.Setup(x => x.GenerateCanceledLinesEmail(It.IsAny<IGrouping<int, CanceledOrderHeaderModel>>())).Returns(new Email { Receivers = ["test1@email.com"] });

        // Act
        await _sendCanceledLineEmails.JobToRun(null, null);

        // Assert
        _emailSender.Verify(x => x.SendEmailAsync(It.IsAny<Email>()), Times.Once);
    }

    [Test]
    public async Task JobToRun_CommitsOrderButDoesNotSendsEmailForEmptyReceivers()
    {
        // Arrange
        var country = BalticCountry.LT;
        var canceledOrders = _fixture.CreateMany<CanceledOrderHeaderModel>(2).ToList();
        canceledOrders[0].Lines.First().EmailStatus = CanceledOrderLineEmailStatus.NotSent;
        canceledOrders[0].SendCanceledOrderNotification = true;
        canceledOrders[1].Lines.First().EmailStatus = CanceledOrderLineEmailStatus.NotSent;
        canceledOrders[1].SendCanceledOrderNotification = true;
        _canceledOrderRepository.Setup(x => x.GetCanceledOrders(country, It.IsAny<List<CanceledOrderLineEmailStatus>>())).ReturnsAsync(canceledOrders);
        _canceledOrderRepository.SetupSequence(x => x.Update(It.IsAny<CanceledOrderHeaderModel>()))
            .ThrowsAsync(new Exception())
            .ReturnsAsync(canceledOrders[1]);
        _canceledOrderLinesEmailGenerator.Setup(x => x.GenerateCanceledLinesEmail(It.IsAny<IGrouping<int, CanceledOrderHeaderModel>>())).Returns(new Email { Receivers = [""] });

        // Act
        await _sendCanceledLineEmails.JobToRun(null, null);

        // Assert
        _emailSender.Verify(x => x.SendEmailAsync(It.IsAny<Email>()), Times.Never);
        _unitOfWork.Verify(x => x.CommitAsync(), Times.Once);
    }

    [Test]
    public async Task JobToRun_CommitsOrdersWhichHasSuccesfullySendEmail()
    {
        // Arrange
        var country = BalticCountry.LT;
        var canceledOrders = _fixture.CreateMany<CanceledOrderHeaderModel>(2).ToList();
        canceledOrders[0].Lines.First().EmailStatus = CanceledOrderLineEmailStatus.NotSent;
        canceledOrders[0].SendCanceledOrderNotification = true;
        canceledOrders[1].Lines.First().EmailStatus = CanceledOrderLineEmailStatus.NotSent;
        canceledOrders[1].SendCanceledOrderNotification = true;
        _canceledOrderRepository.Setup(x => x.GetCanceledOrders(country, It.IsAny<List<CanceledOrderLineEmailStatus>>())).ReturnsAsync(canceledOrders);
        _emailSender.SetupSequence(x => x.SendEmailAsync(It.IsAny<Email>()))
            .ThrowsAsync(new Exception())
            .Returns(Task.CompletedTask);
        _canceledOrderLinesEmailGenerator.Setup(x => x.GenerateCanceledLinesEmail(It.IsAny<IGrouping<int, CanceledOrderHeaderModel>>())).Returns(new Email { Receivers = ["test1@email.com"] });

        // Act
        await _sendCanceledLineEmails.JobToRun(null, null);

        // Assert
        _unitOfWork.Verify(x => x.CommitAsync(), Times.Once);
        _unitOfWork.Verify(x => x.RollbackAsync(), Times.Once);
    }

    [Test]
    public async Task JobToRun_NotifiesOnlyForNewlyIdentifiedNotValidEmailsForSubscribedCustomers()
    {
        // Arrange
        var country = BalticCountry.LV;
        var canceledOrders = _fixture.CreateMany<CanceledOrderHeaderModel>(5).ToList();
        canceledOrders[0].Lines.ToList().ForEach(x => x.EmailStatus = CanceledOrderLineEmailStatus.FailureSending);
        canceledOrders[0].SendCanceledOrderNotification = true;
        canceledOrders[1].Lines.ToList().ForEach(x => x.EmailStatus = CanceledOrderLineEmailStatus.FailureSending);
        canceledOrders[1].SendCanceledOrderNotification = true;
        canceledOrders[2].Lines.ToList().ForEach(x => x.EmailStatus = CanceledOrderLineEmailStatus.NotSent);
        canceledOrders[2].SendCanceledOrderNotification = true;
        canceledOrders[3].Lines.ToList().ForEach(x => x.EmailStatus = CanceledOrderLineEmailStatus.NotSent);
        canceledOrders[3].SendCanceledOrderNotification = true;
        canceledOrders[4].Lines.ToList().ForEach(x => x.EmailStatus = CanceledOrderLineEmailStatus.NotSent);
        canceledOrders[4].SendCanceledOrderNotification = false;
        _canceledOrderRepository.Setup(x => x.GetCanceledOrders(country, It.IsAny<List<CanceledOrderLineEmailStatus>>())).ReturnsAsync(canceledOrders);
        _resolver1.Setup(r => r.Resolve(It.IsAny<IEnumerable<CanceledOrderHeaderModel>>(), country))
            .Callback<IEnumerable<CanceledOrderHeaderModel>, BalticCountry>((orders, _) =>
            {
                var targetOrder = canceledOrders[2];
                foreach (var line in targetOrder.Lines)
                {
                    line.EmailStatus = CanceledOrderLineEmailStatus.FailureSending;
                }

                var targetOrder2 = canceledOrders[3];
                foreach (var line in targetOrder2.Lines)
                {
                    line.EmailStatus = CanceledOrderLineEmailStatus.WillNotBeSent;
                }

                var targetOrder3 = canceledOrders[4];
                foreach (var line in targetOrder3.Lines)
                {
                    line.EmailStatus = CanceledOrderLineEmailStatus.FailureSending;
                }
            })
            .Returns(Task.CompletedTask);

        // Act
        await _sendCanceledLineEmails.JobToRun(null, null);

        // Assert
        _canceledOrderLinesEmailGeneratorFactory.Verify(x => x.Get(country), Times.Once);
        _canceledOrderLinesEmailGenerator.Verify(x => x.GenerateMissingEmailsNotification(It.IsAny<string>(), It.Is<IEnumerable<CanceledOrderHeaderModel>>(x => x.Count() == 1 && x.First().Id == canceledOrders[2].Id)), Times.Once);
        _emailSender.Verify(x => x.SendEmailAsync(It.IsAny<Email>()), Times.Once);
    }

    [Test]
    public async Task JobToRun_WhenNotifying_DecoratesOrdersWithCustomerData()
    {
        // Arrange
        var country = BalticCountry.LV;
        var canceledOrders = _fixture.CreateMany<CanceledOrderHeaderModel>(1).ToList();
        canceledOrders[0].Lines.ToList().ForEach(x => x.EmailStatus = CanceledOrderLineEmailStatus.NotSent);
        canceledOrders[0].SendCanceledOrderNotification = true;

        _canceledOrderRepository.Setup(x => x.GetCanceledOrders(country, It.IsAny<List<CanceledOrderLineEmailStatus>>())).ReturnsAsync(canceledOrders);
        _resolver1.Setup(r => r.Resolve(It.IsAny<IEnumerable<CanceledOrderHeaderModel>>(), country))
            .Callback<IEnumerable<CanceledOrderHeaderModel>, BalticCountry>((orders, _) =>
            {
                var targetOrder = canceledOrders[0];
                foreach (var line in targetOrder.Lines)
                {
                    line.EmailStatus = CanceledOrderLineEmailStatus.FailureSending;
                }
            })
            .Returns(Task.CompletedTask);

        // Act
        await _sendCanceledLineEmails.JobToRun(null, null);

        // Assert
        _canceledOrderCustomerDecorator.Verify(x => x.Decorate(It.Is<IEnumerable<ISalesOrderHeader>>(y => y.Count() == 1), BalticCountry.LV), Times.Once);
        _canceledOrderLinesEmailGeneratorFactory.Verify(x => x.Get(country), Times.Once);
        _canceledOrderLinesEmailGenerator.Verify(x => x.GenerateMissingEmailsNotification(It.IsAny<string>(), It.Is<IEnumerable<CanceledOrderHeaderModel>>(x => x.Count() == 1)), Times.Once);
        _emailSender.Verify(x => x.SendEmailAsync(It.IsAny<Email>()), Times.Once);
    }
}
