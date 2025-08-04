using Tamro.Madam.Application.Services.Sales.HeldOrders.Factories;
using Tamro.Madam.Models.General;
using Tamro.Madam.Models.Sales.HeldOrders;
using Tamroutilities.Email.Sender;
using Tamroutilities.Email.Utils;
using Tamroutilities.Extensions.String;

namespace Tamro.Madam.Application.Services.Sales.HeldOrders;
public class E1HeldOrderEmailService : IE1HeldOrderEmailService
{
    private readonly IE1HeldOrderEmailGeneratorFactory _heldOrderEmailGeneratorFactory;
    private readonly IEmailSender _emailSender;

    public E1HeldOrderEmailService(IE1HeldOrderEmailGeneratorFactory heldOrderEmailGeneratorFactory, IEmailSender emailSender)
    {
        _heldOrderEmailGeneratorFactory = heldOrderEmailGeneratorFactory ?? throw new ArgumentNullException(nameof(heldOrderEmailGeneratorFactory));
        _emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
    }

    public void ResolveEmailValidity(IEnumerable<E1HeldOrderModel> orders)
    {
        foreach (var order in orders)
        {
            order.HasValidCustomerEmails = IsValidEmail(order.Email);
            order.HasValidEmployeeEmails = IsValidEmail(order.EmployeesEmail);
        }
    }

    public async Task SendCustomerEmail(E1HeldOrderModel order, BalticCountry country)
    {
        var email = _heldOrderEmailGeneratorFactory.Get(country).GenerateCustomerEmail(order);
        await _emailSender.SendEmailAsync(email);
    }

    public async Task SendEmployeeEmails(IEnumerable<E1HeldOrderModel> orders, BalticCountry country)
    {
        var ordersGroupedByEmployeeAndShipTo = orders
            .Where(x => x.HasValidEmployeeEmails)
            .GroupBy(order => order.EmployeesEmail)
            .Select(employeeGroup => new
            {
                EmployeesEmail = employeeGroup.Key,
                OrdersByShipTo = employeeGroup.GroupBy(order => order.E1ShipTo)
            });

        var emailGenerator = _heldOrderEmailGeneratorFactory.Get(country);

        foreach (var employeeGroup in ordersGroupedByEmployeeAndShipTo)
        {
            var emails = emailGenerator.GenerateEmployeeEmails(employeeGroup.OrdersByShipTo, employeeGroup.EmployeesEmail);
            foreach (var email in emails)
            {
                await _emailSender.SendEmailAsync(email);
            }
        }
    }

    private static bool IsValidEmail(string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            return false;
        }

        var emailAddresses = email.SplitAndTrim(',', ';');

        return emailAddresses.All(EmailUtils.IsValidEmail);
    }
}
