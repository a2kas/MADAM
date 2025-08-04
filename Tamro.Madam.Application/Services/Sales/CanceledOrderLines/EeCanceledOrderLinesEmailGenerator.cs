using System.Text;
using Tamro.Madam.Models.Sales.CanceledOrderLines;
using Tamroutilities.Email.Models;

namespace Tamro.Madam.Application.Services.Sales.CanceledOrderLines;

public class EeCanceledOrderLinesEmailGenerator : ICanceledOrderLinesEmailGenerator
{
    public Email GenerateCanceledLinesEmail(IGrouping<int, CanceledOrderHeaderModel> ordersbyRecipient)
    {
        var messageBuilder = new StringBuilder();

        messageBuilder.AppendLine("Tere!");
        messageBuilder.AppendLine("<br>");
        messageBuilder.AppendLine("<br>");
        messageBuilder.AppendLine("Anname teada, et allpool loetletud kaupade osas ei saanud me kahjuks Teie tellimust täita ja need read kustutati:");
        messageBuilder.AppendLine("<br>");
        messageBuilder.AppendLine("<br>");
        messageBuilder.AppendLine(@"<table border = ""1"">");
        messageBuilder.AppendLine("<tr>");
        messageBuilder.AppendLine("<th>Tootekood</th>");
        messageBuilder.AppendLine("<th>Tootenimetus</th>");
        messageBuilder.AppendLine("<th>Tellitud kogus</th>");
        messageBuilder.AppendLine("<th>Kustutatud kogus</th>");
        messageBuilder.AppendLine("<th>Kliendi tellimuse number</th>");
        messageBuilder.AppendLine("<th>Tamro tellimuse number</th>");
        messageBuilder.AppendLine("</tr>");

        foreach (var order in ordersbyRecipient)
        {
            var groupedLines = order.Lines
                .GroupBy(line => line.ItemNo)
                .Select(group => new
                {
                    ItemNo2 = group.Key,
                    group.First().ItemName,
                    OrderedQuantity = group.Sum(line => line.OrderedQuantity),
                    CanceledQuantity = group.Sum(line => line.CanceledQuantity)
                });

            foreach (var line in groupedLines)
            {
                messageBuilder.AppendLine("<tr>");
                messageBuilder.AppendLine($"<td>{line.ItemNo2}</td>");
                messageBuilder.AppendLine($"<td>{line.ItemName}</td>");
                messageBuilder.AppendLine($"<td>{line.OrderedQuantity}</td>");
                messageBuilder.AppendLine($"<td>{line.CanceledQuantity}</td>");
                messageBuilder.AppendLine($"<td>{order.CustomerOrderNo}</td>");
                messageBuilder.AppendLine($"<td>{order.DocumentNo}</td>");
                messageBuilder.AppendLine("</tr>");
            }

            foreach (var line in order.Lines)
            {
                line.EmailStatus = CanceledOrderLineEmailStatus.Sent;
            }
        }

        messageBuilder.AppendLine("</table>");
        messageBuilder.AppendLine("<br>");
        messageBuilder.AppendLine("<br>");
        messageBuilder.AppendLine("Vabandame ebamugavuste pärast. Küsimuste korral kirjutage meile myyk@tamro.com.");
        messageBuilder.AppendLine("<br>");
        messageBuilder.AppendLine("<br>");
        messageBuilder.AppendLine("NB! Antud kiri on automaatne teavitus ja palume sellele mitte vastata.");
        messageBuilder.AppendLine("<br>");
        messageBuilder.AppendLine("<br>");
        messageBuilder.AppendLine("Lugupidamisega,");
        messageBuilder.AppendLine("<br>");
        messageBuilder.AppendLine("<br>");
        messageBuilder.AppendLine("Tamro müügiosakond");

        return new Email
        {
            Subject = "Osaliselt täidetud tellimus",
            Receivers = ordersbyRecipient.First().Lines.Select(x => x.EmailAddress).First().Split(',').Select(email => email.Trim()).ToList(),
            Message = messageBuilder.ToString(),
        };
    }

    public Email GenerateMissingEmailsNotification(string responsiblePerson, IEnumerable<CanceledOrderHeaderModel> ordersWithMissingEmail)
    {
        if (ordersWithMissingEmail == null || !ordersWithMissingEmail.Any())
        {
            return default;
        }

        var messageBuilder = new StringBuilder();

        messageBuilder.AppendLine("Hello!");
        messageBuilder.AppendLine("<br>");
        messageBuilder.AppendLine("<br>");
        messageBuilder.AppendLine("Please be informed, the following customer(s) missing e-mail address, or the provided one is incorrect:");
        messageBuilder.AppendLine("<br>");
        messageBuilder.AppendLine("<br>");

        var uniqueOrdersByE1ShipTo = ordersWithMissingEmail.DistinctBy(x => x.E1ShipTo);

        foreach (var uniqueOrder in uniqueOrdersByE1ShipTo)
        {
            messageBuilder.AppendLine($"<b>ShipTo: </b>{uniqueOrder.E1ShipTo}, <b>SoldTo: </b> {uniqueOrder.SoldTo}, <b>Customer name: </b> {uniqueOrder.CustomerName}");
            messageBuilder.AppendLine("<br>");
        }

        messageBuilder.AppendLine("<br>");
        messageBuilder.AppendLine("<br>");
        messageBuilder.AppendLine("Please update the customer e-mail in E1 (select code E - E-mail address).");

        return new Email
        {
            Subject = "Canceled order lines: customer e-mail issue",
            Receivers = [responsiblePerson],
            Message = messageBuilder.ToString(),
        };
    }
}
