using AutoMapper;
using Tamro.Madam.Models.Suppliers;
using Tamro.Madam.Repository.Entities.Suppliers;

namespace Tamro.Madam.Application.Profiles.Suppliers;

public class SupplierProfile : Profile
{
    public SupplierProfile()
    {
        CreateMap<Supplier, SupplierGridModel>();

        CreateMap<Supplier, SupplierDetailsModel>();
        CreateMap<SupplierDetailsModel, Supplier>()
            .ForMember(d => d.RowVer, o => o.Ignore())
            .ForMember(d => d.CreatedDate, o => o.Ignore());
    }
}
