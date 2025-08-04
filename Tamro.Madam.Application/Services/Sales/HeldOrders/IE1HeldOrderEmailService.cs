using Tamro.Madam.Models.General;
using Tamro.Madam.Models.Sales.HeldOrders;

namespace Tamro.Madam.Application.Services.Sales.HeldOrders;
public interface IE1HeldOrderEmailService
{
    void ResolveEmailValidity(IEnumerable<E1HeldOrderModel> orders);
    Task SendCustomerEmail(E1HeldOrderModel order, BalticCountry country);
    Task SendEmployeeEmails(IEnumerable<E1HeldOrderModel> orders, BalticCountry country);
}
