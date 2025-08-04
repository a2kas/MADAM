using Tamro.Madam.Models.Sales.HeldOrders;
using Tamroutilities.Email.Models;

namespace Tamro.Madam.Application.Services.Sales.HeldOrders;
public interface IE1HeldOrderEmailGenerator
{
    Email GenerateCustomerEmail(E1HeldOrderModel order);
    IEnumerable<Email> GenerateEmployeeEmails(IEnumerable<IGrouping<int, E1HeldOrderModel>> ordersGroupedByShipTo, string employeesEmail);
}
