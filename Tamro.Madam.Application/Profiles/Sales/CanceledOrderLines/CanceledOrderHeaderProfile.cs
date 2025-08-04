using AutoMapper;
using Tamro.Madam.Models.Sales.CanceledOrderLines;
using Tamro.Madam.Models.Sales.CanceledOrderLines.Statistics;
using Tamro.Madam.Repository.Entities.Sales.CanceledOrderLines;

namespace Tamro.Madam.Application.Profiles.Sales.CanceledOrderLines;

public class CanceledOrderHeaderProfile : Profile
{
    public CanceledOrderHeaderProfile()
    {
        CreateMap<E1CanceledOrderHeader, CanceledOrderHeaderGridModel>()
            .ForMember(d => d.CustomerName, o => o.Ignore());
        CreateMap<E1CanceledOrderHeader, CanceledOrderHeaderModel>()
            .ForMember(d => d.CustomerName, o => o.Ignore())
            .ForMember(d => d.SoldTo, o => o.Ignore())
            .ForMember(d => d.SendCanceledOrderNotification, o => o.Ignore());
        CreateMap<CanceledOrderHeaderModel, E1CanceledOrderHeader>()
            .ForMember(d => d.CreatedDate, o => o.Ignore())
            .ForMember(d => d.RowVer, o => o.Ignore());
        CreateMap<E1CanceledOrderLine, CanceledOrderLineModel>()
            .ForMember(d => d.ItemNo, o => o.MapFrom(src => src.ItemNo2))
            .ForMember(d => d.ItemName, o => o.Ignore());
        CreateMap<CanceledOrderLineModel, E1CanceledOrderLine>()
            .ForMember(d => d.ItemNo2, o => o.MapFrom(src => src.ItemNo))
            .ForMember(d => d.CreatedDate, o => o.Ignore())
            .ForMember(d => d.RowVer, o => o.Ignore())
            .ForMember(d => d.E1CanceledOrderHeaderId, o => o.Ignore());
        CreateMap<CanceledLineStatistic, CanceledLineStatisticModel>()
            .ForMember(d => d.ItemNo, o => o.MapFrom(src => src.ItemNo2))
            .ForMember(d => d.ItemName, o => o.Ignore())
            .ForMember(d => d.CustomerName, o => o.Ignore());

    }
}
