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

    private readonly ICustomerLegalEntityRepository _customerLegalEntityRepository;
    private readonly ICustomerRepository _customerRepository;

    public CustomerNotificationSettingsResolver(ICustomerLegalEntityRepository customerLegalEntityRepository, ICustomerRepository customerRepository)
    {
        _customerLegalEntityRepository = customerLegalEntityRepository ?? throw new ArgumentNullException(nameof(customerLegalEntityRepository));
        _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
    }

    public async Task Resolve(IEnumerable<CanceledOrderHeaderModel> orders, BalticCountry country)
    {
        foreach (var order in orders)
        {
            order.SendCanceledOrderNotification = true;
        }

        var ordersWithSoldTo = orders.Where(x => x.SoldTo.HasValue);
        var soldTos = ordersWithSoldTo.Select(x => x.SoldTo.Value).Distinct().ToList();

        var includes = new List<IncludeOperation<CustomerLegalEntity>>
        {
            new (q => q.Include(c => c.NotificationSettings)),
        };

        var customer = await _customerRepository.Get(x => x.IsActive && soldTos.Contains(x.E1ShipTo));
        var customerLegalEntities = await _customerLegalEntityRepository.GetMany(x => x.IsActive && soldTos.Contains(x.E1SoldTo) && x.Country == country, includes);
        var customersUnsubscribedFromNotifications = customerLegalEntities.ToDictionary(x => x.E1SoldTo, x => x.NotificationSettings?.SendCanceledOrderNotification);

        foreach (var order in ordersWithSoldTo)
        {
            if (customersUnsubscribedFromNotifications.TryGetValue(order.SoldTo.Value, out var sendCanceledOrderNotification))
            {
                order.SendCanceledOrderNotification = sendCanceledOrderNotification.HasValue && sendCanceledOrderNotification.Value;
            }
        }
    }
}
