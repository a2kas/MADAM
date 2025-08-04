using AutoFixture;
using NUnit.Framework;
using Shouldly;
using Tamro.Madam.Application.Services.Sales.HeldOrders;
using Tamro.Madam.Models.Sales.HeldOrders;

namespace Tamro.Madam.Application.Tests.Services.Sales.HeldOrders;
public class LvHeldOrderEmailGeneratorTests
{
    private Fixture _fixture;
    private LvHeldOrderEmailGenerator _lvHeldOrderEmailGenerator;

    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture();
        _lvHeldOrderEmailGenerator = new LvHeldOrderEmailGenerator();
    }

    [Test]
    public void GenerateCustomerEmail_GeneratesCorrectEmail()
    {
        // Arrange
        var order = _fixture.Create<E1HeldOrderModel>();
        order.DocumentNo = 123;
        order.Email = "customer@tamro.com";
        order.EmployeesEmail = "employee@tamro.com";

        // Act
        var email = _lvHeldOrderEmailGenerator.GenerateCustomerEmail(order);

        // Assert
        email.ShouldNotBeNull();
        email.Subject.ShouldBe($"Paziņojums par minimālo pasūtījuma summu. [{order.DocumentNo}]");
        email.Receivers.ShouldBe(new List<string> { "customer@tamro.com" });
        email.ReceiversBcc.ShouldBe(new List<string> { "employee@tamro.com" });

        var message = email.Message;
        message.ShouldContain($"pasūtījumā [{order.DocumentNo}] netika sasniegta minimālā pasūtījuma summa.");
        message.ShouldContain("Ar cieņu, Jūsu Tamro");
    }

    [Test]
    public void GenerateEmployeeEmails_GeneratesCorrectEmails()
    {
        // Arrange
        var orders = _fixture.CreateMany<E1HeldOrderModel>(3).ToList();
        orders[0].E1ShipTo = 100;
        orders[0].MailingName = "Customer1";
        orders[0].Email = "customer1@tamro.com";
        orders[0].HasValidEmployeeEmails = true;
        orders[0].EmployeesEmail = "employee1@tamro.com, employee2@tamro.com";
        orders[1].E1ShipTo = 100;
        orders[1].MailingName = "Customer1";
        orders[1].Email = "customer1@tamro.com";
        orders[1].EmployeesEmail = "employee1@tamro.com, employee2@tamro.com";
        orders[1].HasValidEmployeeEmails = true;
        orders[2].E1ShipTo = 200;
        orders[2].MailingName = "Customer2";
        orders[2].Email = "customer2@tamro.com";
        orders[2].EmployeesEmail = "employee1@tamro.com, employee2@tamro.com";
        orders[2].HasValidEmployeeEmails = true;

        var ordersGroupedByEmployeeAndShipTo = orders
            .Where(x => x.HasValidEmployeeEmails)
            .GroupBy(order => order.EmployeesEmail)
            .Select(employeeGroup => employeeGroup.GroupBy(order => order.E1ShipTo));

        // Act
        var emails = _lvHeldOrderEmailGenerator.GenerateEmployeeEmails(ordersGroupedByEmployeeAndShipTo.First(), "employee1@tamro.com, employee2@tamro.com").ToList();

        // Assert
        emails.ShouldNotBeNull();
        emails.Count.ShouldBe(2);

        emails[0].Subject.ShouldBe("Held orders: customer e-mail issue");
        emails[0].Receivers.ShouldContain("employee1@tamro.com");

        var message = emails[0].Message;
        message.ShouldContain("Customer1");
        message.ShouldContain("customer1@tamro.com");
        message.ShouldContain("Customer2");
        message.ShouldContain("customer2@tamro.com");

        var customer1Count = message.Split(new[] { "Customer1" }, StringSplitOptions.None).Length - 1;
        customer1Count.ShouldBe(1);

        emails[1].Subject.ShouldBe("Held orders: customer e-mail issue");
        emails[1].Receivers.ShouldContain("employee2@tamro.com");

        var message1 = emails[1].Message;
        message1.ShouldBe(message);
    }
}