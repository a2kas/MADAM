using System.Text;
using Tamro.Madam.Models.Sales.CanceledOrderLines;
using Tamroutilities.Email.Models;

namespace Tamro.Madam.Application.Services.Sales.CanceledOrderLines;

public class LvCanceledOrderLinesEmailGenerator : ICanceledOrderLinesEmailGenerator
{
    public Email GenerateCanceledLinesEmail(IGrouping<int, CanceledOrderHeaderModel> ordersbyRecipient)
    {
        var messageBuilder = new StringBuilder();

        messageBuilder.AppendLine("Labdien!");
        messageBuilder.AppendLine("<br>");
        messageBuilder.AppendLine("<br>");
        messageBuilder.AppendLine("No Jūsu pasūtijuma netiks piegādātas sekojošās rindas:");
        messageBuilder.AppendLine("<br>");
        messageBuilder.AppendLine("<br>");
        messageBuilder.AppendLine(@"<table border = ""1"">");
        messageBuilder.AppendLine("<tr>");
        messageBuilder.AppendLine("<th>Pasūtijuma numurs</th>");
        messageBuilder.AppendLine("<th>Preces kods</th>");
        messageBuilder.AppendLine("<th>Preces nosaukums</th>");
        messageBuilder.AppendLine("<th>Pasūtītais daudzums</th>");
        messageBuilder.AppendLine("<th>Atceltais daudzums</th>");
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
                messageBuilder.AppendLine($"<td>{order.DocumentNo}</td>");
                messageBuilder.AppendLine($"<td>{line.ItemNo2}</td>");
                messageBuilder.AppendLine($"<td>{line.ItemName}</td>");
                messageBuilder.AppendLine($"<td>{line.OrderedQuantity}</td>");
                messageBuilder.AppendLine($"<td>{line.CanceledQuantity}</td>");
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
        messageBuilder.AppendLine("Atvainojamies par sagādātajām neērtībām.");
        messageBuilder.AppendLine("<br>");
        messageBuilder.AppendLine("<br>");
        messageBuilder.AppendLine("Ar cieņu, Jūsu Tamro");
        messageBuilder.AppendLine("<br>");
        messageBuilder.AppendLine("<br>");
        messageBuilder.AppendLine("Šī ziņa ir automātisks paziņojums, lūdzam neatbildēt uz to.");

        return new Email
        {
            Subject = "Informācija par pasūtījuma izpildi",
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
