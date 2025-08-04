using AutoMapper;
using Tamro.Madam.Models.Sales.CanceledOrderLines.ExcludedCustomers;
using Tamro.Madam.Repository.Entities.Customers;

namespace Tamro.Madam.Application.Profiles.Sales.CanceledOrderLines.ExcludedCustomers;

public class ExcludedCustomerProfile : Profile
{
    public ExcludedCustomerProfile()
    {
        CreateMap<CustomerLegalEntity, ExcludedCustomerGridModel>()
            .ForMember(d => d.Name, o => o.Ignore());
    }
}
