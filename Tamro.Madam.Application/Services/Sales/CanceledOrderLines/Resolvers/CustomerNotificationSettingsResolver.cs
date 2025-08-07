using LinqKit;
using Microsoft.EntityFrameworkCore;
using Tamro.Madam.Models.General;
using Tamro.Madam.Models.Sales.CanceledOrderLines;
using Tamro.Madam.Repository.Common;
using Tamro.Madam.Repository.Entities.Customers;
using Tamro.Madam.Repository.Repositories.Customers;

namespace Tamro.Madam.Application.Services.Sales.CanceledOrderLines.Resolvers;

public class CustomerNotificationSettingsResolver : ICanceledOrderLinesResolver
{
    public int Priority => 2;
    private readonly ICustomerRepository _customerRepository;

    public CustomerNotificationSettingsResolver(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
    }

    public async Task Resolve(IEnumerable<CanceledOrderHeaderModel> orders, BalticCountry country)
    {
        orders.ForEach(o => o.SendCanceledOrderNotification = true);
        var e1ShipTos = orders.Select(x => x.E1ShipTo).Distinct().ToList();
        var includes = new List<IncludeOperation<Customer>>
        {
            new(q => q.Include(c => c.CustomerLegalEntity)
                       .ThenInclude(cle => cle.NotificationSettings)),
            new(q => q.Include(c => c.CustomerNotification))
        };

        var customers = await _customerRepository.GetMany(
            x => x.IsActive && e1ShipTos.Contains(x.E1ShipTo),
            includes,
            track: false);

        var customerNotificationSettings = customers.ToDictionary(
            x => x.E1ShipTo,
            x => GetNotificationSetting(x));

        foreach (var order in orders)
        {
            if (customerNotificationSettings.TryGetValue(order.E1ShipTo, out var shouldSendNotification))
            {
                order.SendCanceledOrderNotification = shouldSendNotification;
            }
        }
    }

    private static bool GetNotificationSetting(Customer customer)
    {
        if (customer.CustomerNotification != null)
        {
            return customer.CustomerNotification.SendCanceledOrderNotification;
        }

        if (customer.CustomerLegalEntity?.NotificationSettings != null)
        {
            return customer.CustomerLegalEntity.NotificationSettings.SendCanceledOrderNotification;
        }

        return true;
    }
}
