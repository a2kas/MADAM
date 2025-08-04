using AutoMapper;
using Tamro.Madam.Models.Customers;
using Tamro.Madam.Repository.Entities.Customers;

namespace Tamro.Madam.Application.Profiles.Customers;

public class CustomerProfile : Profile
{
    public CustomerProfile()
    {
        CreateMap<CustomerLegalEntity, CustomerLegalEntityModel>();
        CreateMap<CustomerLegalEntityNotification, CustomerLegalEntityNotificationModel>();
    }
}
