using AutoMapper;
using Tamro.Madam.Models.ItemMasterdata.Items.Bindings.Vlk;
using Tamro.Madam.Models.ItemMasterdata.Items.Bindings;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items.Bindings.Vlk;

namespace Tamro.Madam.Application.Profiles.ItemMasterdata.Items.Bindings.Vlk;

public class VlkBindingProfile : Profile
{
    public VlkBindingProfile()
    {
        CreateMap<VlkBinding, VlkBindingGridModel>()
            .ForMember(d => d.ItemNo2, o => o.MapFrom(src => src.ItemBinding.LocalId))
            .ForMember(d => d.ItemName, o => o.MapFrom(src => src.ItemBinding.Item.ItemName));

        CreateMap<VlkBindingGridModel, VlkBindingDetailsModel>()
            .ForMember(d => d.ItemBinding, o => o.MapFrom(src => new ItemBindingClsfModel()
            {
                Id = src.ItemBindingId,
                ItemNo2 = src.ItemNo2,
                Name = src.ItemName,
            }));

        CreateMap<VlkBindingDetailsModel, VlkBinding>()
            .ForMember(d => d.ItemBindingId, o => o.MapFrom(src => src.ItemBinding.Id))
            .ForMember(d => d.ItemBinding, o => o.Ignore());
    }
}
