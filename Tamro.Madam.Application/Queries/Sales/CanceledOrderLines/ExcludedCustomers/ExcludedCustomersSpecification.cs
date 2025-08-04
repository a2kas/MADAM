using Ardalis.Specification;
using Tamro.Madam.Application.Extensions.Specification;
using Tamro.Madam.Application.Queries.Sales.CanceledOrderLines.ExcludedCustomers;
using Tamro.Madam.Models.Sales.CanceledOrderLines.ExcludedCustomers;
using Tamro.Madam.Repository.Entities.Customers;

namespace Tamro.Madam.Application.Queries.Sales.CanceledOrderLines;

public class ExcludedCustomersSpecification : Specification<CustomerLegalEntity>
{
    public ExcludedCustomersSpecification(ExcludedCustomersFilter filter)
    {
        Query.Where(x => !x.NotificationSettings.SendCanceledOrderNotification);

        if (filter.Country != null)
        {
            Query.Where(x => x.Country == filter.Country);
        }

        if (filter.Filters == null)
        {
            return;
        }

        foreach (var appliedFilter in filter.Filters)
        {
            if (appliedFilter == null || appliedFilter.Column == null)
            {
                continue;
            }

            if (appliedFilter.Column.PropertyName.Equals(nameof(ExcludedCustomerGridModel.E1SoldTo)))
            {
                Query.ApplyIntFilter(appliedFilter.Operator, appliedFilter.Value, x => x.E1SoldTo);
            }
        }
    }
}
