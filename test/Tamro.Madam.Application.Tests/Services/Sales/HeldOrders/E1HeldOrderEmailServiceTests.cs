using AutoFixture;
using Moq;
using NUnit.Framework;
using Shouldly;
using Tamro.Madam.Application.Services.Sales.HeldOrders;
using Tamro.Madam.Application.Services.Sales.HeldOrders.Factories;
using Tamro.Madam.Models.General;
using Tamro.Madam.Models.Sales.HeldOrders;
using Tamroutilities.Email.Models;
using Tamroutilities.Email.Sender;

namespace Tamro.Madam.Application.Tests.Services.Sales.HeldOrders;
[TestFixture]
public class E1HeldOrderEmailServiceTests
{
    private Fixture _fixture;

    private Mock<IE1HeldOrderEmailGenerator> _e1HeldOrderEmailGenerator;

    private Mock<IE1HeldOrderEmailGeneratorFactory> _e1HeldOrderEmailGeneratorFactory;
    private Mock<IEmailSender> _emailSender;

    private E1HeldOrderEmailService _e1HeldOrderEmailService;

    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture();

        _e1HeldOrderEmailGenerator = new Mock<IE1HeldOrderEmailGenerator>();

        _e1HeldOrderEmailGeneratorFactory = new Mock<IE1HeldOrderEmailGeneratorFactory>();
        _emailSender = new Mock<IEmailSender>();

        _e1HeldOrderEmailService = new E1HeldOrderEmailService(_e1HeldOrderEmailGeneratorFactory.Object, _emailSender.Object);
    }

    [TestCase("some@email.com, another@email.com", "some@email.com, another@email.com", true, true)]
    [TestCase("some(email)_some14@email.com, another@email.com", "some@email.com, some(email)_some14@email.com", false, false)]
    [TestCase("someemail.com, another@email.com", "", true, false)]
    [TestCase("someemail.com, another@email.com; @notvalidemail.com", "some@email.com, another@emailcom", false, true)]
    public void ResolveEmailValidity_ValidatesCustomerAndEmployeesEmails_Correctly(string customerEmail, string employeeEmail, bool expectedHasValidCustomerEmails, bool expectedHasValidEmployeeEmails)
    {
        // Arrange
        var orders = _fixture.CreateMany<E1HeldOrderModel>(1).ToList();
        orders[0].Email = customerEmail;
        orders[0].EmployeesEmail = employeeEmail;

        // Act
        _e1HeldOrderEmailService.ResolveEmailValidity(orders);

        // Assert
        orders[0].HasValidCustomerEmails.ShouldBe(expectedHasValidCustomerEmails);
        orders[0].HasValidEmployeeEmails.ShouldBe(expectedHasValidEmployeeEmails);
    }

    [Test]
    public async Task SendCustomerEmail_GeneratesEmailByFactoryAndSendEmail()
    {
        // Arrange
        var country = BalticCountry.LV;
        _e1HeldOrderEmailGeneratorFactory.Setup(x => x.Get(country)).Returns(_e1HeldOrderEmailGenerator.Object);
        var order = _fixture.Create<E1HeldOrderModel>();
        var email = new Email
        {
            Subject = "Customer email",
        };
        _e1HeldOrderEmailGenerator.Setup(x => x.GenerateCustomerEmail(order)).Returns(email);

        // Act
        await _e1HeldOrderEmailService.SendCustomerEmail(order, country);

        // Assert
        _e1HeldOrderEmailGeneratorFactory.Verify(x => x.Get(country), Times.Once);
        _e1HeldOrderEmailGenerator.Verify(x => x.GenerateCustomerEmail(order), Times.Once);
        _emailSender.Verify(x => x.SendEmailAsync(email), Times.Once);
    }

    [Test]
    public async Task SendEmployeeEmails_GeneratesEmailsByFactoryAndSendEmailsOncePerEmployee()
    {
        // Arrange
        var country = BalticCountry.LV;
        _e1HeldOrderEmailGeneratorFactory.Setup(x => x.Get(country)).Returns(_e1HeldOrderEmailGenerator.Object);
        var orders = _fixture.CreateMany<E1HeldOrderModel>(8).ToList();
        orders[0].HasValidEmployeeEmails = false;
        orders[1].HasValidEmployeeEmails = true;
        orders[2].HasValidEmployeeEmails = true;
        orders[3].HasValidEmployeeEmails = true;
        orders[4].HasValidEmployeeEmails = true;
        orders[5].HasValidEmployeeEmails = true;
        orders[6].HasValidEmployeeEmails = false;
        orders[7].HasValidEmployeeEmails = true;
        orders[1].EmployeesEmail = orders[2].EmployeesEmail;
        orders[4].EmployeesEmail = orders[5].EmployeesEmail;

        var email1 = new List<Email> { new(), new() };
        var email2 = new List<Email> { new() };
        var email3 = new List<Email> { new(), new(), new() };
        var email4 = new List<Email> { new() };

        _e1HeldOrderEmailGenerator.SetupSequence(x => x.GenerateEmployeeEmails(It.IsAny<IEnumerable<IGrouping<int, E1HeldOrderModel>>>(), It.IsAny<string>()))
            .Returns(email1)
            .Returns(email2)
            .Returns(email3)
            .Returns(email4);

        // Act
        await _e1HeldOrderEmailService.SendEmployeeEmails(orders, country);

        // Assert
        _e1HeldOrderEmailGeneratorFactory.Verify(x => x.Get(country), Times.Once);
        _e1HeldOrderEmailGenerator.Verify(x => x.GenerateEmployeeEmails(It.Is<IEnumerable<IGrouping<int, E1HeldOrderModel>>>(y => y.Count() == 2), It.IsAny<string>()), Times.Exactly(2));
        _e1HeldOrderEmailGenerator.Verify(x => x.GenerateEmployeeEmails(It.Is<IEnumerable<IGrouping<int, E1HeldOrderModel>>>(y => y.Count() == 1), It.IsAny<string>()), Times.Exactly(2));
        _emailSender.Verify(x => x.SendEmailAsync(It.IsAny<Email>()), Times.Exactly(7));
    }
}
