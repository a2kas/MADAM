using Tamro.Madam.Models.Sales.CanceledOrderLines;
using Tamroutilities.Email.Models;

namespace Tamro.Madam.Application.Services.Sales.CanceledOrderLines;

public interface ICanceledOrderLinesEmailGenerator
{
    Email GenerateCanceledLinesEmail(IGrouping<int, CanceledOrderHeaderModel> ordersbyRecipient);
    Email GenerateMissingEmailsNotification(string responsiblePerson, IEnumerable<CanceledOrderHeaderModel> ordersWithMissingEmail);
}
