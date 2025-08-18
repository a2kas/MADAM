using Ardalis.Specification;
using Tamro.Madam.Application.Extensions.Specification;
using Tamro.Madam.Application.Queries.Sales.CanceledOrderLines.ExcludedCustomers;
using Tamro.Madam.Models;
using Tamro.Madam.Models.Sales.CanceledOrderLines.ExcludedCustomers;
using Tamro.Madam.Repository.Entities.Customers;

namespace Tamro.Madam.Application.Queries.Sales.CanceledOrderLines;

public class ExcludedCustomersSpecification : Specification<CustomerLegalEntity>
{
    public ExcludedCustomersSpecification(ExcludedCustomersFilter filter)
    {
        Query.Where(x =>
            (x.NotificationSettings != null && x.NotificationSettings.SendCanceledOrderNotification == false) ||
            (x.Customers != null && x.Customers.Any(c =>
                c.CustomerNotification != null &&
                c.CustomerNotification.SendCanceledOrderNotification == false))
        );

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
            else if (appliedFilter.Column.PropertyName.Equals(nameof(ExcludedCustomerGridModel.ExclusionLevel)))
            {
                if (appliedFilter.Value is string filterValue && !string.IsNullOrEmpty(filterValue))
                {
                    if (Enum.TryParse<ExclusionLevel>(filterValue, out var exclusionLevel))
                    {
                        if (exclusionLevel == ExclusionLevel.EntireLegalEntity)
                        {
                            Query.Where(x => x.NotificationSettings != null &&
                                            x.NotificationSettings.SendCanceledOrderNotification == false);
                        }
                        else if (exclusionLevel == ExclusionLevel.OneOrMorePhysicalLocations)
                        {
                            Query.Where(x => x.Customers != null &&
                                            x.Customers.Any(c => c.CustomerNotification != null &&
                                                               c.CustomerNotification.SendCanceledOrderNotification == false) &&
                                            (x.NotificationSettings == null || x.NotificationSettings.SendCanceledOrderNotification != false));
                        }
                    }
                }

                else if (appliedFilter.Value is IEnumerable<object> values)
                {
                    var exclusionLevels = values
                        .Select(v => v?.ToString())
                        .Where(s => !string.IsNullOrEmpty(s) && Enum.TryParse<ExclusionLevel>(s, out _))
                        .Select(s => Enum.Parse<ExclusionLevel>(s))
                        .ToList();

                    if (exclusionLevels.Any())
                    {
                        var hasEntireLegalEntity = exclusionLevels.Contains(ExclusionLevel.EntireLegalEntity);
                        var hasPhysicalLocations = exclusionLevels.Contains(ExclusionLevel.OneOrMorePhysicalLocations);

                        if (hasEntireLegalEntity && hasPhysicalLocations)
                        {
                            // Both types - use existing base filter (no additional filtering needed)
                        }
                        else if (hasEntireLegalEntity)
                        {
                            Query.Where(x => x.NotificationSettings != null &&
                                            x.NotificationSettings.SendCanceledOrderNotification == false);
                        }
                        else if (hasPhysicalLocations)
                        {
                            Query.Where(x => x.Customers != null &&
                                            x.Customers.Any(c => c.CustomerNotification != null &&
                                                               c.CustomerNotification.SendCanceledOrderNotification == false) &&
                                            (x.NotificationSettings == null || x.NotificationSettings.SendCanceledOrderNotification != false));
                        }
                    }
                }
            }
        }
    }
}