using AutoMapper;
using Tamro.Madam.Models.Customers.Wholesale;
using Tamro.Madam.Models.Customers.Wholesale.Clsf;
using Tamro.Madam.Repository.Entities.Customers.Sks;
using Tamro.Madam.Repository.Entities.Wholesale.Ee;
using Tamro.Madam.Repository.Entities.Wholesale.Lt;
using Tamro.Madam.Repository.Entities.Wholesale.Lv;

namespace Tamro.Madam.Application.Profiles.Customers.Wholesale;

public class WholesaleCustomerProfile : Profile
{
    public WholesaleCustomerProfile()
    {
        CreateMap<LvWholesaleCustomer, WholesaleCustomerClsfModel>()
            .ForMember(d => d.Name, o => o.MapFrom(src => src.MailingName));
        CreateMap<LtWholesaleCustomer, WholesaleCustomerClsfModel>()
            .ForMember(d => d.Name, o => o.MapFrom(src => src.MailingName));
        CreateMap<EeWholesaleCustomer, WholesaleCustomerClsfModel>()
            .ForMember(d => d.Name, o => o.MapFrom(src => src.MailingName));
        CreateMap<LvWholesaleCustomer, WholesaleCustomerModel>()
            .ForMember(d => d.EmailAddress, o => o.MapFrom(src => src.ElectronicAddress))
            .ForMember(d => d.LegalEntityNumber, o => o.MapFrom(src => src.AddressNumber2));
        CreateMap<EeWholesaleCustomer, WholesaleCustomerModel>()
            .ForMember(d => d.EmailAddress, o => o.MapFrom(src => src.ElectronicAddress))
            .ForMember(d => d.LegalEntityNumber, o => o.MapFrom(src => src.AddressNumber2))
            .ForMember(d => d.ResponsibleEmployeeNumber, o => o.Ignore());
        CreateMap<OrderNotificationEmail, WholesaleCustomerModel>()
            .ForMember(d => d.EmailAddress, o => o.MapFrom(src => src.Email))
            .ForMember(d => d.LegalEntityNumber, o => o.MapFrom(src => src.AddressNumber))
            .ForMember(d => d.ResponsibleEmployeeNumber, o => o.Ignore())
            .ForMember(d => d.MailingName, o => o.Ignore());
    }
}
