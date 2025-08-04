using AutoMapper;
using Tamro.Madam.Models.Administration.Configuration.Ubl;
using Tamro.Madam.Models.Customers.Wholesale.Clsf;
using Tamro.Madam.Repository.Entities.Administration.Configuration.Ubl;

namespace Tamro.Madam.Application.Profiles.Administration.Configuration;
public class UblApiKeyProfile : Profile
{
    public UblApiKeyProfile()
    {
        CreateMap<UblApiKey, UblApiKeyModel>()
            .ForMember(d => d.ApiKey, o => o.MapFrom(s => "********-****-****-****-********"));
        CreateMap<UblApiKeyModel, UblApiKey>();
        CreateMap<UblApiKeyModel, UblApiKeyEditModel>()
            .ForMember(d => d.ApiKey, o => o.Ignore())
            .ForMember(d => d.Country, o => o.Ignore())
            .ForMember(d => d.Customer, o => o.MapFrom(s => new WholesaleCustomerClsfModel()
            {
                AddressNumber = s.E1SoldTo,
                Name = s.CustomerName,
            }));
        CreateMap<UblApiKeyEditModel, UblApiKeyModel>()
            .ForMember(d => d.E1SoldTo, o => o.MapFrom(s => s.Customer.AddressNumber))
            .ForMember(d => d.CustomerName, o => o.MapFrom(s => s.Customer.Name));
    }
}
