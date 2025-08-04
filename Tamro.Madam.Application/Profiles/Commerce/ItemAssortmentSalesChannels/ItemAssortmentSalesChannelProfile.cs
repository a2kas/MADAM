using AutoMapper;
using Tamro.Madam.Models.Commerce.ItemAssortmentSalesChannels;
using Tamro.Madam.Repository.Entities.Commerce.Assortment;

namespace Tamro.Madam.Application.Profiles.Commerce.ItemAssortmentSalesChannels;

public class ItemAssortmentSalesChannelProfile : Profile
{
    public ItemAssortmentSalesChannelProfile()
    {
        CreateMap<ItemAssortmentSalesChannel, ItemAssortmentSalesChannelGridModel>()
            .ForMember(d => d.ItemsCount, o => o.MapFrom(src => src.ItemAssortmentBindingMaps.Count));

        CreateMap<ItemAssortmentSalesChannelDetailsModel, ItemAssortmentSalesChannel>()
            .ForMember(d => d.CreatedDate, o => o.Ignore())
            .ForMember(d => d.RowVer, o => o.Ignore())
            .ForMember(d => d.ItemAssortmentBindingMaps, o => o.MapFrom(src => src.Assortment));

        CreateMap<ItemAssortmentSalesChannel, ItemAssortmentSalesChannelDetailsModel>()
            .ForMember(d => d.Assortment, o => o.MapFrom(src => src.ItemAssortmentBindingMaps));
    }
}
