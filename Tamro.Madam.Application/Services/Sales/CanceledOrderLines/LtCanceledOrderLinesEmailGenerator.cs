using System.Text;
using Tamro.Madam.Models.Sales.CanceledOrderLines;
using Tamroutilities.Email.Models;

namespace Tamro.Madam.Application.Services.Sales.CanceledOrderLines;

public class LtCanceledOrderLinesEmailGenerator : ICanceledOrderLinesEmailGenerator
{
    public Email GenerateCanceledLinesEmail(IGrouping<int, CanceledOrderHeaderModel> ordersbyRecipient)
    {
        var messageBuilder = new StringBuilder();

        messageBuilder.AppendLine("Sveiki,");
        messageBuilder.AppendLine("<br>");
        messageBuilder.AppendLine("Informuojame, kad žemiau nurodytoms prekėms negalime įvykdyti Jūsų užsakymo:");
        messageBuilder.AppendLine("<br>");
        messageBuilder.AppendLine("<br>");
        messageBuilder.AppendLine(@"<table border = ""1"">");
        messageBuilder.AppendLine("<tr>");
        messageBuilder.AppendLine("<th>Prekė</th>");
        messageBuilder.AppendLine("<th>Atšauktas kiekis</th>");
        messageBuilder.AppendLine("<th>Užsakytas kiekis</th>");
        messageBuilder.AppendLine("<th>Užsakymo numeris</th>");
        messageBuilder.AppendLine("<th>Tamro numeris</th>");
        messageBuilder.AppendLine("</tr>");

        foreach (var order in ordersbyRecipient)
        {
            var groupedLines = order.Lines
                .GroupBy(line => line.ItemNo)
                .Select(group => new
                {
                    ItemNo2 = group.Key,
                    group.First().ItemName,
                    group.First().OrderedQuantity,
                    CanceledQuantity = group.Sum(line => line.CanceledQuantity)
                });

            foreach (var line in groupedLines)
            {
                messageBuilder.AppendLine("<tr>");
                messageBuilder.AppendLine($"<td>{line.ItemName} ({line.ItemNo2})</td>");
                messageBuilder.AppendLine($"<td>{line.CanceledQuantity} vnt.</td>");
                messageBuilder.AppendLine($"<td>{line.OrderedQuantity} vnt.</td>");
                messageBuilder.AppendLine($"<td>{RemoveASKS(order.CustomerOrderNo)}</td>");
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
        messageBuilder.AppendLine("Atsiprašome už nepatogumus.");
        messageBuilder.AppendLine("<br>");
        messageBuilder.AppendLine("<br>");
        messageBuilder.AppendLine("Jei turite klausimų, prašome kreiptis prekyba@tamro.com.");
        messageBuilder.AppendLine("<br>");
        messageBuilder.AppendLine("<br>");
        messageBuilder.AppendLine("Jūsų Tamro");

        var msg = messageBuilder.ToString();

        return new Email
        {
            Subject = "TAMRO: Nepilnai įvykdytas užsakymas",
            Receivers = [ordersbyRecipient.First().Lines.Select(x => x.EmailAddress).First()],
            Message = messageBuilder.ToString(),
        };
    }

    private static string RemoveASKS(string customerOrderNo)
    {
        if (string.IsNullOrEmpty(customerOrderNo))
        {
            return customerOrderNo;
        }

        const string prefix = "ASKS-";

        if (customerOrderNo.StartsWith(prefix))
        {
            return customerOrderNo.Substring(prefix.Length);
        }

        return customerOrderNo;
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

        var e1ShipTos = ordersWithMissingEmail.Select(x => x.E1ShipTo).Distinct();

        foreach (var e1ShipTo in e1ShipTos)
        {
            messageBuilder.AppendLine($"{e1ShipTo}");
            messageBuilder.AppendLine("<br>");
        }

        messageBuilder.AppendLine("<br>");
        messageBuilder.AppendLine("<br>");
        messageBuilder.AppendLine("Please update the entry here: http://appserv/intranet/sales/orders_reply_emails.aspx");

        return new Email
        {
            Subject = "Canceled order lines: customer e-mail issue",
            Receivers = [responsiblePerson],
            Message = messageBuilder.ToString(),
        };
    }
}
