using AutoMapper;
using Tamro.Madam.Models.Customers.Wholesale.Clsf;
using Tamro.Madam.Repository.Entities.Customers.E1;

namespace Tamro.Madam.Application.Profiles.Customers.Wholesale;

public class E1CustomerProfile : Profile
{
    public E1CustomerProfile()
    {
        CreateMap<Customer, WholesaleCustomerClsfModel>()
            .ForMember(d => d.Name, o => o.MapFrom(src => src.MailingName));
    }
}
