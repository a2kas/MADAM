using AutoMapper;
using Tamro.Madam.Models.Customers.Wholesale.Clsf;
using Tamro.Madam.Models.Sales.Sabis;
using Tamro.Madam.Repository.Entities.Sales.Sabis;

namespace Tamro.Madam.Application.Profiles.Sales.Sabis;

public class SksContractProfile : Profile
{
    public SksContractProfile()
    {
        CreateMap<SksContractMapping, SksContractGridModel>()
            .ForMember(d => d.CustomerName, o => o.MapFrom(src => src.Customer != null ? src.Customer.MailingName : null))
            .ForMember(d => d.CompanyId, o => o.MapFrom(src => src.AdditionalTaxId))
            .ForMember(d => d.EditedAt, o => o.MapFrom(src => src.RowVer));

        CreateMap<SksContractModel, SksContractMapping>()
            .ForMember(d => d.AddressNumber, o => o.MapFrom(src => src.Customer != null ? src.Customer.AddressNumber : default))
            .ForMember(d => d.AdditionalTaxId, o => o.MapFrom(src => src.CompanyId))
            .ForMember(d => d.RowVer, o => o.MapFrom(src => src.EditedAt))
            .ForMember(d => d.Customer, o => o.Ignore());

        CreateMap<SksContractGridModel, SksContractModel>()
            .ForMember(d => d.Customer, o => o.MapFrom(src => new WholesaleCustomerClsfModel()
            {
                AddressNumber = src.AddressNumber,
                Name = src.CustomerName,
            }));
    }
}
