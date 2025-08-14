using AutoMapper;
using Tamro.Madam.Models;
using Tamro.Madam.Models.Sales.CanceledOrderLines.ExcludedCustomers;
using Tamro.Madam.Repository.Entities.Customers;

namespace Tamro.Madam.Application.Profiles.Sales.CanceledOrderLines.ExcludedCustomers;

public class ExcludedCustomerProfile : Profile
{
    public ExcludedCustomerProfile()
    {
        CreateMap<CustomerLegalEntity, ExcludedCustomerGridModel>()
            .ForMember(d => d.Name, o => o.Ignore())
            .ForMember(d => d.ExclusionLevel, o => o.MapFrom(src => GetExclusionLevel(src)));
    }

    private static ExclusionLevel GetExclusionLevel(CustomerLegalEntity entity)
    {
        if (entity.NotificationSettings != null && entity.NotificationSettings.SendCanceledOrderNotification == false)
        {
            return ExclusionLevel.EntireLegalEntity;
        }

        if (entity.Customers != null && entity.Customers.Any(c =>
            c.CustomerNotification != null &&
            c.CustomerNotification.SendCanceledOrderNotification == false))
        {
            return ExclusionLevel.OneOrMorePhysicalLocations;
        }

        return ExclusionLevel.None;
    }
}
