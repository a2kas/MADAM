using AutoMapper;
using Tamro.Madam.Models.Sales.CanceledOrderLines;
using Tamro.Madam.Repository.Entities.Sales.CanceledOrderLines;

namespace Tamro.Madam.Application.Profiles.Sales.CanceledOrderLines;

public class CanceledOrderLineProfile : Profile
{
    public CanceledOrderLineProfile()
    {
        CreateMap<E1CanceledOrderLine, CanceledOrderLineGridModel>()
            .ForMember(d => d.ItemNo, o => o.MapFrom(src => src.ItemNo2))
            .ForMember(d => d.ItemName, o => o.Ignore());
    }
}
