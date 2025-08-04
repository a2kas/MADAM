using AutoMapper;
using Tamro.Madam.Models.ItemMasterdata.Items.Wholesale;
using Tamro.Madam.Models.ItemMasterdata.Items.Wholesale.Clsf;
using Tamro.Madam.Repository.Entities.Wholesale.Ee;
using Tamro.Madam.Repository.Entities.Wholesale.Lt;
using Tamro.Madam.Repository.Entities.Wholesale.Lv;

namespace Tamro.Madam.Application.Profiles.ItemMasterdata.Items.Wholesale;
public class WholesaleItemProfile : Profile
{
    public WholesaleItemProfile()
    {
        CreateMap<LtWholesaleItem, WholesaleItemClsfModel>()
            .ForMember(d => d.Name, o => o.MapFrom(src => src.ItemDescription))
            .ForMember(d => d.DisplayName, o => o.Ignore());
        CreateMap<LvWholesaleItem, WholesaleItemClsfModel>()
            .ForMember(d => d.Name, o => o.MapFrom(src => src.ItemDescription))
            .ForMember(d => d.DisplayName, o => o.Ignore());
        CreateMap<EeWholesaleItem, WholesaleItemClsfModel>()
            .ForMember(d => d.Name, o => o.MapFrom(src => src.ItemDescription.Trim()))
            .ForMember(d => d.ItemNo, o => o.MapFrom(src => src.ItemNo.Trim()))
            .ForMember(d => d.DisplayName, o => o.Ignore());
        CreateMap<LvWholesaleItem, WholesaleItemModel>();
        CreateMap<LtWholesaleItem, WholesaleItemModel>();
        CreateMap<EeWholesaleItem, WholesaleItemModel>()
            .ForMember(d => d.ItemDescription, o => o.MapFrom(src => src.ItemDescription.Trim()))
            .ForMember(d => d.ItemNo, o => o.MapFrom(src => src.ItemNo.Trim()));
    }
}