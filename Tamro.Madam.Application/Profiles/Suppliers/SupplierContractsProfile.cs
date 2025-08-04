using AutoMapper;
using Tamro.Madam.Models.Suppliers;
using Tamro.Madam.Repository.Entities.Suppliers;

namespace Tamro.Madam.Application.Profiles.Suppliers;

public class SupplierContractsProfile : Profile
{
    public SupplierContractsProfile()
    {
        CreateMap<SupplierContractModel, SupplierContract>()
            .ForMember(d => d.CreatedDate, o => o.Ignore())
            .ForMember(d => d.RowVer, o => o.Ignore())
            .ForMember(d => d.SupplierId, o => o.Ignore());

        CreateMap<SupplierContract, SupplierContractModel>()
            .ForMember(d => d.Guid, o => o.Ignore());
        CreateMap<SupplierContractModel, SupplierContractModel>();
    }
}

