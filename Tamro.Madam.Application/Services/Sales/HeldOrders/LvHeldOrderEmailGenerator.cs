using System.Text;
using Tamro.Madam.Models.Sales.HeldOrders;
using Tamroutilities.Email.Models;
using Tamroutilities.Extensions.String;

namespace Tamro.Madam.Application.Services.Sales.HeldOrders;
public class LvHeldOrderEmailGenerator : IE1HeldOrderEmailGenerator
{
    public Email GenerateCustomerEmail(E1HeldOrderModel order)
    {
        var messageBuilder = new StringBuilder();

        messageBuilder.AppendLine("Sveicināti,");
        messageBuilder.AppendLine("<br>");
        messageBuilder.AppendLine("<br>");
        messageBuilder.AppendLine($"Vēlamies Jūs informēt, ka pasūtījumā [{order.DocumentNo}] netika sasniegta minimālā pasūtījuma summa.");
        messageBuilder.AppendLine("<br>");
        messageBuilder.AppendLine("Lūdzam Jūs sazināties ar savu Klientu apkalpošanas speciālistu, lai papildinātu pasūtījumu.");
        messageBuilder.AppendLine("<br>");
        messageBuilder.AppendLine("Vai zvaniet uz kopējo numuru +371 670 678 30.");
        messageBuilder.AppendLine("Ja pasūtījums netiks papildināts un, netiks sasniegta pasūtījuma minimālā summa 100.00 (viens simts eiro), Jūsu pasūtījums tiks atcelts.");
        messageBuilder.AppendLine("<br>");
        messageBuilder.AppendLine("Ar cieņu, Jūsu Tamro");
        messageBuilder.AppendLine("<br>");
        messageBuilder.AppendLine("<br>");
        messageBuilder.AppendLine("Šī ziņa ir automātisks paziņojums, lūdzam neatbildēt uz to.");

        var orderRecipients = order.Email.SplitAndTrim(',', ';');
        var orderEmployeeRecipients = order.EmployeesEmail.SplitAndTrim(',', ';');

        return new Email
        {
            Subject = $"Paziņojums par minimālo pasūtījuma summu. [{order.DocumentNo}]",
            Receivers = orderRecipients,
            ReceiversBcc = orderEmployeeRecipients,
            Message = messageBuilder.ToString(),
        };
    }

    public IEnumerable<Email> GenerateEmployeeEmails(IEnumerable<IGrouping<int, E1HeldOrderModel>> ordersGroupedByShipTo, string employeesEmail)
    {
        var result = new List<Email>();
        var messageBuilder = new StringBuilder();

        messageBuilder.AppendLine("Please be informed, the following customer(s) got order(s) on hold, but missing e-mail address, or the provided is incorrect:");
        messageBuilder.AppendLine("<br>");
        messageBuilder.AppendLine("<br>");
        messageBuilder.AppendLine(@"<table border = ""1"">");
        messageBuilder.AppendLine("<tr>");
        messageBuilder.AppendLine("<th>ShipTo</th>");
        messageBuilder.AppendLine("<th>Customer name</th>");
        messageBuilder.AppendLine("<th>Email addresses</th>");
        messageBuilder.AppendLine("</tr>");

        foreach (var shipToGroup in ordersGroupedByShipTo)
        {
            var firstOrder = shipToGroup.First();
            messageBuilder.AppendLine("<tr>");
            messageBuilder.AppendLine($"<td>{firstOrder.E1ShipTo}</td>");
            messageBuilder.AppendLine($"<td>{firstOrder.MailingName}</td>");
            messageBuilder.AppendLine($"<td>{string.Join(", ", shipToGroup.Select(order => order.Email).Distinct())}</td>");
            messageBuilder.AppendLine("</tr>");
        }

        messageBuilder.AppendLine("</table>");
        messageBuilder.AppendLine("<br>");
        messageBuilder.AppendLine("Please update the customer(s) e-mail in E1 in Customer Address Book within several days (select Electronic Address Type: E-mail address), otherwise no notification mail about held order(s) will be sent.");

        var orderEmployeeRecipients = employeesEmail.SplitAndTrim(',', ';');

        foreach (var orderEmployeeRecipient in orderEmployeeRecipients)
        {
            result.Add(new Email
            {
                Subject = "Held orders: customer e-mail issue",
                Receivers = new List<string> { orderEmployeeRecipient },
                Message = messageBuilder.ToString(),
            });
        }

        return result;
    }
}
