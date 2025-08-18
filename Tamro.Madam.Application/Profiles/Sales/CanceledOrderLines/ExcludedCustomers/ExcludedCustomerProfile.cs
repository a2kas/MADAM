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
        var hasLegalEntityExclusion = entity.NotificationSettings?.SendCanceledOrderNotification == false;
        var hasPhysicalLocationExclusions = entity.Customers?.Any(c =>
            c.CustomerNotification?.SendCanceledOrderNotification == false) == true;

        if (hasLegalEntityExclusion)
        {
            return ExclusionLevel.EntireLegalEntity;
        }

        if (hasPhysicalLocationExclusions)
        {
            return ExclusionLevel.OneOrMorePhysicalLocations;
        }

        if (entity.NotificationSettings != null ||
            (entity.Customers?.Any(c => c.CustomerNotification != null) == true))
        {
            return ExclusionLevel.None;
        }

        return ExclusionLevel.None;
    }
}
