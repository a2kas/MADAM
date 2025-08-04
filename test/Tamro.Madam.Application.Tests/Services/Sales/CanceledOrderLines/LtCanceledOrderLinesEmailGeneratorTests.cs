using AutoFixture;
using Shouldly;
using NUnit.Framework;
using Tamro.Madam.Application.Services.Sales.CanceledOrderLines;
using Tamro.Madam.Models.Sales.CanceledOrderLines;

namespace Tamro.Madam.Application.Tests.Services.Sales.CanceledOrderLines;

[TestFixture]
public class LtCanceledOrderLinesEmailGeneratorTests
{
    private Fixture _fixture;
    private LtCanceledOrderLinesEmailGenerator _ltCanceledOrderLinesEmailGenerator;

    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture();

        _ltCanceledOrderLinesEmailGenerator = new LtCanceledOrderLinesEmailGenerator();
    }

    [Test]
    public void GenerateCanceledLinesEmail_GeneratesCorrectEmail()
    {
        // Arrange
        var orders = _fixture.CreateMany<CanceledOrderHeaderModel>(2).ToList();
        orders[0].E1ShipTo = 100;
        orders[0].CustomerOrderNo = "ASKS-555555";
        orders[0].DocumentNo = "SomeDocumentNo1";
        orders[0].Lines.ElementAt(0).ItemNo = "Item1";
        orders[0].Lines.ElementAt(0).ItemName = "Item1Description";
        orders[0].Lines.ElementAt(0).CanceledQuantity = 100000;
        orders[0].Lines.ElementAt(0).OrderedQuantity = 1000000;
        orders[0].Lines.ElementAt(0).EmailAddress = "someemail@tamro.com";
        orders[0].Lines.ElementAt(1).ItemNo = "Item1";
        orders[0].Lines.ElementAt(1).CanceledQuantity = 2;
        orders[0].Lines.ElementAt(1).OrderedQuantity = 1000000;
        orders[0].Lines.ElementAt(2).ItemNo = "Item2";
        orders[0].Lines.ElementAt(2).ItemName = "Item2Description";
        orders[0].Lines.ElementAt(2).CanceledQuantity = 300000;
        orders[0].Lines.ElementAt(2).OrderedQuantity = 3000000;
        orders[1].E1ShipTo = 100;
        orders[1].CustomerOrderNo = "7777777";
        orders[1].DocumentNo = "SomeDocumentNo2";

        var ordersByRecipient = orders.GroupBy(o => o.E1ShipTo).First();

        // Act
        var email = _ltCanceledOrderLinesEmailGenerator.GenerateCanceledLinesEmail(ordersByRecipient);

        // Assert
        email.ShouldNotBeNull();
        email.Subject.ShouldBe("TAMRO: Nepilnai įvykdytas užsakymas");
        email.Receivers.Count.ShouldBe(1);
        email.Receivers.First().ShouldBe("someemail@tamro.com");

        var message = email.Message;
        message.ShouldContain("Item1Description (Item1)");
        message.ShouldContain("100002");
        message.ShouldContain("1000000");
        message.ShouldContain("555555");
        message.ShouldContain("SomeDocumentNo1");

        message.ShouldContain("Item2Description (Item2)");
        message.ShouldContain("SomeDocumentNo2");
        message.ShouldContain("300000");
        message.ShouldContain("3000000");
    }

    [Test]
    public void GenerateMissingEmailsNotification_GeneratesCorrectNotification()
    {
        // Arrange
        var responsiblePerson = _fixture.Create<string>();
        var ordersWithMissingEmail = _fixture.CreateMany<CanceledOrderHeaderModel>(3).ToList();

        // Act
        var email = _ltCanceledOrderLinesEmailGenerator.GenerateMissingEmailsNotification(responsiblePerson, ordersWithMissingEmail);

        // Assert
        email.ShouldNotBeNull();
        email.Subject.ShouldBe("Canceled order lines: customer e-mail issue");
        email.Receivers.Count.ShouldBe(1);
        email.Receivers[0].ShouldBe(responsiblePerson);

        var message = email.Message;
        var distinctE1ShipTos = ordersWithMissingEmail.Select(x => x.E1ShipTo).Distinct();
        foreach (var e1ShipTo in distinctE1ShipTos)
        {
            message.ShouldContain(e1ShipTo.ToString());
        }
    }

    [Test]
    public void GenerateMissingEmailsNotification_NoOrders_ReturnsDefaultEmail()
    {
        // Arrange
        var responsiblePerson = _fixture.Create<string>();
        var ordersWithMissingEmail = Enumerable.Empty<CanceledOrderHeaderModel>();

        // Act
        var email = _ltCanceledOrderLinesEmailGenerator.GenerateMissingEmailsNotification(responsiblePerson, ordersWithMissingEmail);

        // Assert
        email.ShouldBeNull();
    }
}