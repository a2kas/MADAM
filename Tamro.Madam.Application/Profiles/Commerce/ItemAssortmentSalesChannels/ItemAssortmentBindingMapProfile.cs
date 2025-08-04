using AutoMapper;
using Tamro.Madam.Models.Commerce.ItemAssortmentSalesChannels;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items.Bindings.Assortment;

namespace Tamro.Madam.Application.Profiles.Commerce.ItemAssortmentSalesChannels;

public class ItemAssortmentBindingMapProfile : Profile
{
    public ItemAssortmentBindingMapProfile()
    {
        CreateMap<ItemAssortmentBindingMap, ItemAssortmentGridModel>()
            .ForMember(d => d.ItemCode, o => o.MapFrom(src => src.ItemBinding.LocalId))
            .ForMember(d => d.ItemName, o => o.MapFrom(src => src.ItemBinding.Item.ItemName));

        CreateMap<ItemAssortmentGridModel, ItemAssortmentBindingMap>()
            .ForMember(d => d.CreatedDate, o => o.Ignore())
            .ForMember(d => d.RowVer, o => o.Ignore())
            .ForMember(d => d.ItemBinding, o => o.Ignore())
            .ForMember(d => d.ItemAssortmentSalesChannel, o => o.Ignore());
    }
}
